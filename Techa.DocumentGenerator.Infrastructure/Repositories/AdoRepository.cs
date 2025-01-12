using Microsoft.Extensions.Configuration;
using System.Data;
using System.Data.SqlClient;
using Techa.DocumentGenerator.Application.Dtos.DbInfo;
using Techa.DocumentGenerator.Application.Repositories;
using Techa.DocumentGenerator.Domain.Entities.BaseInfo;
using Techa.DocumentGenerator.Infrastructure.Utilities;

namespace Techa.DocumentGenerator.Infrastructure.Repositories
{
    public class AdoRepository : IAdoRepository
    {
        private string _connectionString;

        private SQLQueryDisplayDto result;
        private readonly IBaseRepository<Project> _projectRepository;

        public AdoRepository(IConfiguration configuration, IBaseRepository<Project> projectRepository)
        {
            _connectionString = configuration.GetConnectionString("ProjectDbConnection") ?? 
                "Data Source={0};Initial Catalog={1};Integrated Security=False;User ID={2};password={3}";

            result = new();
            _projectRepository = projectRepository;
        }

        private async Task SetConnectionString(int projectId, CancellationToken cancellationToken)
        {
            var project = await _projectRepository.GetByIdAsync(cancellationToken, projectId);
            if (project == null)
                throw new Exception("پروژه مورد نظر یافت نشد، لذا امکان ایجاد رشته اتصال به بانک اطلاعاتی ممکن نیست.");

            _connectionString = string.Format(_connectionString, project.DbServerName, project.DbName, project.DbUserName, project.DbPassword);
        }

        public async Task<SQLQueryDisplayDto> GetDataAsync(int projectId, string query, bool? autoCloseConnection, CancellationToken cancellationToken)
        {
            await SetConnectionString(projectId, cancellationToken);

            SqlConnection sqlcon = new SqlConnection(_connectionString);
            await sqlcon.OpenAsync();

            sqlcon.FireInfoMessageEventOnUserErrors = true;
            sqlcon.InfoMessage += sqlConnection_InfoMessage;

            SqlDataAdapter sqlda;

            SqlCommand sqlcom = new SqlCommand(query, sqlcon);
            sqlcom.StatementCompleted += sqlConnection_OnStatementCompleted;

            sqlda = new SqlDataAdapter(sqlcom);

            DataSet dt = new DataSet();
            sqlda.Fill(dt);

            if (autoCloseConnection ?? true)
                await sqlcon.CloseAsync();

            result.Script = query;
            result.Dataset = dt.ConvertDataSetToString();

            return result;
        }

        public async Task<SQLQueryDisplayDto> SetDataAsync(int projectId, string query, bool? autoCloseConnection, CancellationToken cancellationToken)
        {
            await SetConnectionString(projectId, cancellationToken);

            SqlConnection sqlcon = new SqlConnection(_connectionString);
            await sqlcon.OpenAsync();

            sqlcon.FireInfoMessageEventOnUserErrors = true;
            sqlcon.InfoMessage += sqlConnection_InfoMessage;

            SqlCommand sqlcom = new SqlCommand(query, sqlcon);

            sqlcom.StatementCompleted += sqlConnection_OnStatementCompleted;

            await sqlcom.ExecuteNonQueryAsync();

            if (autoCloseConnection ?? true)
                await sqlcon.CloseAsync();

            result.Script = query;
            result.Dataset = null;

            return result;
        }

        private void sqlConnection_OnStatementCompleted(object sender, StatementCompletedEventArgs e)
        {
            AddMessage(String.Format("({0} row(s) affected)", e.RecordCount), false);
        }

        private void sqlConnection_InfoMessage(object sender, SqlInfoMessageEventArgs e)
        {
            bool err = false;
            foreach (System.Data.SqlClient.SqlError sqlEvent in e.Errors)
            {
                string strErr;
                strErr = String.Format("Msg {0}, Level {1}, State {2}, Line {3}{4}"
                    , sqlEvent.Number, sqlEvent.Class, sqlEvent.State, sqlEvent.LineNumber, Environment.NewLine);

                if ((sqlEvent.Number != 0 || sqlEvent.Class != 0 || sqlEvent.Message.Equals(e.Message) == false)
                    && sqlEvent.Message.Contains("Caution:") == false
                    && sqlEvent.Message.Contains("percent processed") == false
                    && sqlEvent.Message.Contains("pages for database") == false
                    && sqlEvent.Message.Contains("BACKUP DATABASE successfully") == false
                    && sqlEvent.Message.Contains("Changed database context") == false
                    && sqlEvent.Message.Contains("RESTORE DATABASE successfully") == false

                    )
                {
                    AddMessage(strErr, true);
                    result.Messages += strErr;
                    result.HasError = true;
                    err = true;
                }
            }

            AddMessage(e.Message, err);
        }

        private void AddMessage(string msg, bool isError)
        {
            if (string.IsNullOrWhiteSpace(msg))
                result.Messages = msg;
            else
                result.Messages += Environment.NewLine + msg;

            result.HasError = isError;
        }

    }
}
