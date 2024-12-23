using Pluralize.NET;
using System.Reflection;
using System.Text;
using Techa.DocumentGenerator.CodeGeneratore.Utilities;
using Techa.DocumentGenerator.Domain.Entities;
using Techa.DocumentGenerator.Domain.Entities.AAA;

namespace Techa.DocumentGenerator.CodeGeneratore
{
    internal class Program
    {
        static async Task Main(string[] args)
        {
            var yes_no_input = string.Empty;
            do
            {
                Console.WriteLine("Do you want to generate all files based of entities? Yes[Y]/No[N]");
                yes_no_input = Console.ReadLine();
            }
            while (string.IsNullOrEmpty(yes_no_input) || yes_no_input.ToLower() != "y");

            yes_no_input = string.Empty;
            List<string> excludedEntities = new()
            {
                nameof(Role).ToLower(),
                nameof(User).ToLower(),
                nameof(UserRole).ToLower(),

                nameof(AttachmentFile).ToLower(),
                nameof(EventLog).ToLower()
            };

            do
            {
                Console.WriteLine("Please enter Entity Name to exclude. Cancel[C]");
                yes_no_input = Console.ReadLine();

                if (!string.IsNullOrEmpty(yes_no_input) && yes_no_input.ToLower() != "c")
                    excludedEntities.Add(yes_no_input.ToLower());
            }
            while (string.IsNullOrEmpty(yes_no_input) || yes_no_input.ToLower() != "c");

            (string root_Directory, 
                string solutionName, 
                string applicationLayer_Directory, 
                string apiLayer_Directory, 
                IEnumerable<Type> entities) applicationInfos = GetProjectDirectories(excludedEntities);

            foreach (var entity in applicationInfos.entities)
            {
                Console.WriteLine($"Generating '{entity.Name}' require files");

                var entityFolderName = entity.Namespace.Split('.').Last();

                #region DTO Files

                {
                    var dtosPath = Path.Combine(applicationInfos.applicationLayer_Directory, "Dtos");

                    if (entityFolderName != "Entities")
                        dtosPath = Path.Combine(dtosPath, entityFolderName);

                    if (!Directory.Exists(dtosPath))
                        Directory.CreateDirectory(dtosPath);

                    var validatorPath = Path.Combine(dtosPath, "Validators");
                    if (!Directory.Exists(validatorPath))
                        Directory.CreateDirectory(validatorPath);

                    CreateDtoAndReplaceMarkedValues(dtosPath, entity, "Create", applicationInfos.solutionName, entityFolderName);
                    CreateDtoAndReplaceMarkedValues(dtosPath, entity, "Display", applicationInfos.solutionName, entityFolderName);
                    CreateDtoAndReplaceMarkedValues(dtosPath, entity, "Search", applicationInfos.solutionName, entityFolderName);
                    
                    CreateValidatorAndReplaceMarkedValues(validatorPath, entity, "Create", applicationInfos.solutionName, entityFolderName);
                    CreateValidatorAndReplaceMarkedValues(validatorPath, entity, "Search", applicationInfos.solutionName, entityFolderName);

                }

                #endregion

                #region CQRS Files

                {
                    //// [ Start Generate CQRS Directories ] ////

                    var cqrsPath = Path.Combine(applicationInfos.applicationLayer_Directory, "CQRS");

                    if (entityFolderName != "Entities")
                        cqrsPath = Path.Combine(cqrsPath, entityFolderName);

                    if (!Directory.Exists(cqrsPath))
                        Directory.CreateDirectory(cqrsPath);

                    var filesPath = Path.Combine(cqrsPath, $"{entity.Name}Files");
                    if (!Directory.Exists(filesPath))
                        Directory.CreateDirectory(filesPath);

                    var commandsPath = Path.Combine(filesPath, "Commands");
                    if (!Directory.Exists(commandsPath))
                        Directory.CreateDirectory(commandsPath);

                    var queriesPath = Path.Combine(filesPath, "Queries");
                    if (!Directory.Exists(queriesPath))
                        Directory.CreateDirectory(queriesPath);

                    var handlersPath = Path.Combine(filesPath, "Handlers");
                    if (!Directory.Exists(handlersPath))
                        Directory.CreateDirectory(handlersPath);

                    //// [ End Generate CQRS Directories ] ////

                    //// [ Start Generate Command Files ] ////
                    
                    CreateAndUpdateCommandFilesAndReplaceMarkedValues(commandsPath, entity, "Create", applicationInfos.solutionName, entityFolderName);
                    CreateAndUpdateCommandFilesAndReplaceMarkedValues(commandsPath, entity, "Update", applicationInfos.solutionName, entityFolderName);

                    DeleteCommandFileAndReplaceMarkedValues(commandsPath, entity, applicationInfos.solutionName, entityFolderName);

                    //// [ End Generate Command Files ] ////


                    //// [ Start Generate Query Files ] ////

                    GetQueryFileAndReplaceMarkedValues(queriesPath, entity, applicationInfos.solutionName, entityFolderName);
                    GetAllQueryFileAndReplaceMarkedValues(queriesPath, entity, applicationInfos.solutionName, entityFolderName);

                    //// [ End Generate Query Files ] ////

                    //// [ Start Generate Handler Files ] ////

                    CreateHandlerFileAndReplaceMarkedValues(handlersPath, entity, applicationInfos.solutionName, entityFolderName);
                    UpdateHandlerFileAndReplaceMarkedValues(handlersPath, entity, applicationInfos.solutionName, entityFolderName);
                    DeleteHandlerFileAndReplaceMarkedValues(handlersPath, entity, applicationInfos.solutionName, entityFolderName);
                    GetHandlerFileAndReplaceMarkedValues(handlersPath, entity, applicationInfos.solutionName, entityFolderName);
                    GetAllHandlerFileAndReplaceMarkedValues(handlersPath, entity, applicationInfos.solutionName, entityFolderName);

                    //// [ End Generate Handler Files ] ////
                }

                #endregion

                #region Controllers

                {
                    var apisPath = Path.Combine(applicationInfos.apiLayer_Directory, "Controllers");

                    if (entityFolderName != "Entities")
                        apisPath = Path.Combine(apisPath, entityFolderName);

                    if (!Directory.Exists(apisPath))
                        Directory.CreateDirectory(apisPath);

                    ControllerFileAndReplaceMarkedValues(apisPath, entity, applicationInfos.solutionName, entityFolderName);
                }

                #endregion

            }

            Console.WriteLine("All Required files successfuly generated !!!");
            Console.ReadKey();
        }


        private static void ControllerFileAndReplaceMarkedValues(string apisPath, Type entity, string solutionName, string entityFolderName)
        {
            Pluralizer pluralizer = new Pluralizer();

            if (File.Exists(Path.Combine(apisPath, $"{entity.Name}Controller.cs")))
                return;

            if (DotNetCliHelper.RunCliCommand($"dotnet new class -n {entity.Name}Controller", apisPath))
            {
                var sourceFilePath = System.IO.Path.Combine(Environment.CurrentDirectory, "TemplateFiles", "ControllerTemplate.txt");
                var sourceFileText = File.ReadAllText(sourceFilePath);

                var queryFileNamespace = $"{solutionName}.Application.CQRS.";
                var commandFileNamespace = $"{solutionName}.Application.CQRS.";
                var controllerFileNamespace = $"{solutionName}.API.Controllers";

                if (entityFolderName != "Entities")
                {
                    queryFileNamespace += $"{entityFolderName}.";
                    commandFileNamespace += $"{entityFolderName}.";
                    controllerFileNamespace += $".{entityFolderName}";

                    sourceFileText = sourceFileText.Replace("{3}", $"{solutionName}.Application.Dtos.{entityFolderName}");
                }
                else
                {
                    sourceFileText = sourceFileText.Replace("{3}", $"{solutionName}.Application.Dtos");
                }

                queryFileNamespace += $"{entity.Name}Files.Queries";
                commandFileNamespace += $"{entity.Name}Files.Commands";

                sourceFileText = sourceFileText.Replace("{1}", $"{commandFileNamespace}");
                sourceFileText = sourceFileText.Replace("{2}", $"{queryFileNamespace}");

                sourceFileText = sourceFileText.Replace("{4}", $"{controllerFileNamespace}");

                sourceFileText = sourceFileText.Replace("{5}", entity.Name);
                sourceFileText = sourceFileText.Replace("{6}", pluralizer.Pluralize(entity.Name));
                sourceFileText = sourceFileText.Replace("{0}", solutionName);

                var commandFilePath = System.IO.Path.Combine(apisPath, $"{entity.Name}Controller.cs");
                System.IO.File.WriteAllText(commandFilePath, sourceFileText, System.Text.Encoding.UTF8);
            }
        }



        private static void CreateAndUpdateCommandFilesAndReplaceMarkedValues(string commadsPath, Type entity, string commandType, string solutionName, string entityFolderName)
        {
            if (File.Exists(Path.Combine(commadsPath, $"{commandType}{entity.Name}Command.cs")))
                return;

            if (DotNetCliHelper.RunCliCommand($"dotnet new class -n {commandType}{entity.Name}Command", commadsPath))
            {
                var sourceFilePath = System.IO.Path.Combine(Environment.CurrentDirectory, "TemplateFiles", "CreateCommandTemplate.txt");
                var sourceFileText = File.ReadAllText(sourceFilePath);

                var commandFileNamespace = $"{solutionName}.Application.CQRS.";

                if (entityFolderName != "Entities")
                {
                    commandFileNamespace += $"{entityFolderName}.";
                    sourceFileText = sourceFileText.Replace("{0}", $"{solutionName}.Application.Dtos.{entityFolderName}");
                }
                else
                {
                    sourceFileText = sourceFileText.Replace("{0}", $"{solutionName}.Application.Dtos");
                }

                commandFileNamespace += $"{entity.Name}Files.Commands";

                sourceFileText = sourceFileText.Replace("{1}", $"{commandFileNamespace}");
                sourceFileText = sourceFileText.Replace("{2}", $"{entity.Name}");
                sourceFileText = sourceFileText.Replace("{3}", $"{commandType}");

                var commandFilePath = System.IO.Path.Combine(commadsPath, $"{commandType}{entity.Name}Command.cs");
                System.IO.File.WriteAllText(commandFilePath, sourceFileText, System.Text.Encoding.UTF8);
            }
        }
        
        private static void CreateHandlerFileAndReplaceMarkedValues(string handlersPath, Type entity, string solutionName, string entityFolderName)
        {
            if (File.Exists(Path.Combine(handlersPath, $"Create{entity.Name}CommandHandler.cs")))
                return;

            if (DotNetCliHelper.RunCliCommand($"dotnet new class -n Create{entity.Name}CommandHandler", handlersPath))
            {
                var sourceFilePath = System.IO.Path.Combine(Environment.CurrentDirectory, "TemplateFiles", "CreateCommandHandlerTemplate.txt");
                var sourceFileText = File.ReadAllText(sourceFilePath);

                var commandFileNamespace = $"{solutionName}.Application.CQRS.";

                if (entityFolderName != "Entities")
                {
                    commandFileNamespace += $"{entityFolderName}.";
                    sourceFileText = sourceFileText.Replace("{1}", $"{solutionName}.Application.Dtos.{entityFolderName}");
                    sourceFileText = sourceFileText.Replace("{3}", $"{solutionName}.Domain.Entities.{entityFolderName}");
                }
                else
                {
                    sourceFileText = sourceFileText.Replace("{1}", $"{solutionName}.Application.Dtos");
                    sourceFileText = sourceFileText.Replace("{3}", $"{solutionName}.Domain.Entities");
                }

                var handlerFileNamespace = commandFileNamespace + $"{entity.Name}Files.Handlers";
                commandFileNamespace += $"{entity.Name}Files.Commands";

                sourceFileText = sourceFileText.Replace("{0}", $"{commandFileNamespace}");
                sourceFileText = sourceFileText.Replace("{4}", $"{handlerFileNamespace}");
                
                sourceFileText = sourceFileText.Replace("{5}", $"{entity.Name}");
                sourceFileText = sourceFileText.Replace("{7}", solutionName);

                var commandFilePath = System.IO.Path.Combine(handlersPath, $"Create{entity.Name}CommandHandler.cs");
                System.IO.File.WriteAllText(commandFilePath, sourceFileText, System.Text.Encoding.UTF8);
            }
        }
        
        private static void UpdateHandlerFileAndReplaceMarkedValues(string handlersPath, Type entity, string solutionName, string entityFolderName)
        {
            if (File.Exists(Path.Combine(handlersPath, $"Update{entity.Name}CommandHandler.cs")))
                return;

            if (DotNetCliHelper.RunCliCommand($"dotnet new class -n Update{entity.Name}CommandHandler", handlersPath))
            {
                var sourceFilePath = System.IO.Path.Combine(Environment.CurrentDirectory, "TemplateFiles", "UpdateCommandHandlerTemplate.txt");
                var sourceFileText = File.ReadAllText(sourceFilePath);

                var commandFileNamespace = $"{solutionName}.Application.CQRS.";

                if (entityFolderName != "Entities")
                {
                    commandFileNamespace += $"{entityFolderName}.";
                    sourceFileText = sourceFileText.Replace("{1}", $"{solutionName}.Application.Dtos.{entityFolderName}");
                    sourceFileText = sourceFileText.Replace("{3}", $"{solutionName}.Domain.Entities.{entityFolderName}");
                }
                else
                {
                    sourceFileText = sourceFileText.Replace("{1}", $"{solutionName}.Application.Dtos");
                    sourceFileText = sourceFileText.Replace("{3}", $"{solutionName}.Domain.Entities");
                }

                var handlerFileNamespace = commandFileNamespace + $"{entity.Name}Files.Handlers";
                commandFileNamespace += $"{entity.Name}Files.Commands";

                sourceFileText = sourceFileText.Replace("{0}", $"{commandFileNamespace}");
                sourceFileText = sourceFileText.Replace("{4}", $"{handlerFileNamespace}");
                
                sourceFileText = sourceFileText.Replace("{5}", $"{entity.Name}");
                sourceFileText = sourceFileText.Replace("{7}", solutionName);

                var commandFilePath = System.IO.Path.Combine(handlersPath, $"Update{entity.Name}CommandHandler.cs");
                System.IO.File.WriteAllText(commandFilePath, sourceFileText, System.Text.Encoding.UTF8);
            }
        }
        
        private static void DeleteHandlerFileAndReplaceMarkedValues(string handlersPath, Type entity, string solutionName, string entityFolderName)
        {
            if (File.Exists(Path.Combine(handlersPath, $"Delete{entity.Name}CommandHandler.cs")))
                return;

            if (DotNetCliHelper.RunCliCommand($"dotnet new class -n Delete{entity.Name}CommandHandler", handlersPath))
            {
                var sourceFilePath = System.IO.Path.Combine(Environment.CurrentDirectory, "TemplateFiles", "DeleteCommandHandlerTemplate.txt");
                var sourceFileText = File.ReadAllText(sourceFilePath);

                var commandFileNamespace = $"{solutionName}.Application.CQRS.";

                if (entityFolderName != "Entities")
                {
                    commandFileNamespace += $"{entityFolderName}.";
                    sourceFileText = sourceFileText.Replace("{1}", $"{solutionName}.Domain.Entities.{entityFolderName}");
                }
                else
                {
                    sourceFileText = sourceFileText.Replace("{1}", $"{solutionName}.Domain.Entities");
                }

                var handlerFileNamespace = commandFileNamespace + $"{entity.Name}Files.Handlers";
                commandFileNamespace += $"{entity.Name}Files.Commands";

                sourceFileText = sourceFileText.Replace("{0}", $"{commandFileNamespace}");
                sourceFileText = sourceFileText.Replace("{2}", $"{handlerFileNamespace}");
                
                sourceFileText = sourceFileText.Replace("{3}", $"{entity.Name}");
                sourceFileText = sourceFileText.Replace("{7}", solutionName);

                var commandFilePath = System.IO.Path.Combine(handlersPath, $"Delete{entity.Name}CommandHandler.cs");
                System.IO.File.WriteAllText(commandFilePath, sourceFileText, System.Text.Encoding.UTF8);
            }
        }

        private static void GetHandlerFileAndReplaceMarkedValues(string handlersPath, Type entity, string solutionName, string entityFolderName)
        {
            if (File.Exists(Path.Combine(handlersPath, $"Get{entity.Name}QueryHandler.cs")))
                return;

            if (DotNetCliHelper.RunCliCommand($"dotnet new class -n Get{entity.Name}QueryHandler", handlersPath))
            {
                var sourceFilePath = System.IO.Path.Combine(Environment.CurrentDirectory, "TemplateFiles", "GetQueryHandlerTemplate.txt");
                var sourceFileText = File.ReadAllText(sourceFilePath);

                var queryFileNamespace = $"{solutionName}.Application.CQRS.";

                if (entityFolderName != "Entities")
                {
                    queryFileNamespace += $"{entityFolderName}.";
                    sourceFileText = sourceFileText.Replace("{1}", $"{solutionName}.Application.Dtos.{entityFolderName}");
                    sourceFileText = sourceFileText.Replace("{4}", $"{solutionName}.Domain.Entities.{entityFolderName}");
                }
                else
                {
                    sourceFileText = sourceFileText.Replace("{1}", $"{solutionName}.Application.Dtos");
                    sourceFileText = sourceFileText.Replace("{4}", $"{solutionName}.Domain.Entities");
                }

                var handlerFileNamespace = queryFileNamespace + $"{entity.Name}Files.Handlers";
                queryFileNamespace += $"{entity.Name}Files.Queries";

                sourceFileText = sourceFileText.Replace("{0}", $"{queryFileNamespace}");
                sourceFileText = sourceFileText.Replace("{2}", $"{handlerFileNamespace}");

                sourceFileText = sourceFileText.Replace("{3}", $"{entity.Name}");
                sourceFileText = sourceFileText.Replace("{7}", solutionName);

                var commandFilePath = System.IO.Path.Combine(handlersPath, $"Get{entity.Name}QueryHandler.cs");
                System.IO.File.WriteAllText(commandFilePath, sourceFileText, System.Text.Encoding.UTF8);
            }
        }

        private static void GetAllHandlerFileAndReplaceMarkedValues(string handlersPath, Type entity, string solutionName, string entityFolderName)
        {
            Pluralizer pluralizer = new Pluralizer();

            if (File.Exists(Path.Combine(handlersPath, $"GetAll{pluralizer.Pluralize(entity.Name)}QueryHandler.cs")))
                return;

            if (DotNetCliHelper.RunCliCommand($"dotnet new class -n GetAll{pluralizer.Pluralize(entity.Name)}QueryHandler", handlersPath))
            {
                var sourceFilePath = System.IO.Path.Combine(Environment.CurrentDirectory, "TemplateFiles", "GetAllQueryHandlerTemplate.txt");
                var sourceFileText = File.ReadAllText(sourceFilePath);

                var queryFileNamespace = $"{solutionName}.Application.CQRS.";

                if (entityFolderName != "Entities")
                {
                    queryFileNamespace += $"{entityFolderName}.";
                    sourceFileText = sourceFileText.Replace("{1}", $"{solutionName}.Application.Dtos.{entityFolderName}");
                    sourceFileText = sourceFileText.Replace("{2}", $"{solutionName}.Domain.Entities.{entityFolderName}");
                }
                else
                {
                    sourceFileText = sourceFileText.Replace("{1}", $"{solutionName}.Application.Dtos");
                    sourceFileText = sourceFileText.Replace("{2}", $"{solutionName}.Domain.Entities");
                }

                var handlerFileNamespace = queryFileNamespace + $"{entity.Name}Files.Handlers";
                queryFileNamespace += $"{entity.Name}Files.Queries";

                sourceFileText = sourceFileText.Replace("{0}", $"{queryFileNamespace}");
                sourceFileText = sourceFileText.Replace("{3}", $"{handlerFileNamespace}");

                sourceFileText = sourceFileText.Replace("{4}", pluralizer.Pluralize(entity.Name));
                sourceFileText = sourceFileText.Replace("{5}", entity.Name);
                sourceFileText = sourceFileText.Replace("{7}", solutionName);

                var commandFilePath = System.IO.Path.Combine(handlersPath, $"GetAll{pluralizer.Pluralize(entity.Name)}QueryHandler.cs");
                System.IO.File.WriteAllText(commandFilePath, sourceFileText, System.Text.Encoding.UTF8);
            }
        }

        private static void DeleteCommandFileAndReplaceMarkedValues(string commadsPath, Type entity, string solutionName, string entityFolderName)
        {
            if (File.Exists(Path.Combine(commadsPath, $"Delete{entity.Name}Command.cs")))
                return;

            if (DotNetCliHelper.RunCliCommand($"dotnet new class -n Delete{entity.Name}Command", commadsPath))
            {
                var sourceFilePath = System.IO.Path.Combine(Environment.CurrentDirectory, "TemplateFiles", "DeleteCommandTemplate.txt");
                var sourceFileText = File.ReadAllText(sourceFilePath);

                var commandFileNamespace = $"{solutionName}.Application.CQRS.";

                if (entityFolderName != "Entities")
                {
                    commandFileNamespace += $"{entityFolderName}.";
                }

                commandFileNamespace += $"{entity.Name}Files.Commands";

                sourceFileText = sourceFileText.Replace("{0}", $"{commandFileNamespace}");
                sourceFileText = sourceFileText.Replace("{1}", $"{entity.Name}");

                var commandFilePath = System.IO.Path.Combine(commadsPath, $"Delete{entity.Name}Command.cs");
                System.IO.File.WriteAllText(commandFilePath, sourceFileText, System.Text.Encoding.UTF8);
            }
        }

        private static void GetQueryFileAndReplaceMarkedValues(string queriesPath, Type entity, string solutionName, string entityFolderName)
        {
            if (File.Exists(Path.Combine(queriesPath, $"Get{entity.Name}Query.cs")))
                return;

            if (DotNetCliHelper.RunCliCommand($"dotnet new class -n Get{entity.Name}Query", queriesPath))
            {
                var sourceFilePath = System.IO.Path.Combine(Environment.CurrentDirectory, "TemplateFiles", "GetQueryTemplate.txt");
                var sourceFileText = File.ReadAllText(sourceFilePath);

                var queryFileNamespace = $"{solutionName}.Application.CQRS.";

                if (entityFolderName != "Entities")
                {
                    queryFileNamespace += $"{entityFolderName}.";
                    sourceFileText = sourceFileText.Replace("{0}", $"{solutionName}.Application.Dtos.{entityFolderName}");
                }
                else
                {
                    sourceFileText = sourceFileText.Replace("{0}", $"{solutionName}.Application.Dtos");
                }

                queryFileNamespace += $"{entity.Name}Files.Queries";

                sourceFileText = sourceFileText.Replace("{1}", $"{queryFileNamespace}");
                sourceFileText = sourceFileText.Replace("{2}", $"{entity.Name}");

                var queryFilePath = System.IO.Path.Combine(queriesPath, $"Get{entity.Name}Query.cs");
                System.IO.File.WriteAllText(queryFilePath, sourceFileText, System.Text.Encoding.UTF8);
            }
        }

        private static void GetAllQueryFileAndReplaceMarkedValues(string queriesPath, Type entity, string solutionName, string entityFolderName)
        {
            Pluralizer pluralizer = new Pluralizer();

            if (File.Exists(Path.Combine(queriesPath, $"GetAll{pluralizer.Pluralize(entity.Name)}Query.cs")))
                return;

            if (DotNetCliHelper.RunCliCommand($"dotnet new class -n GetAll{pluralizer.Pluralize(entity.Name)}Query", queriesPath))
            {
                var sourceFilePath = System.IO.Path.Combine(Environment.CurrentDirectory, "TemplateFiles", "GetAllQueryTemplate.txt");
                var sourceFileText = File.ReadAllText(sourceFilePath);

                var queryFileNamespace = $"{solutionName}.Application.CQRS.";

                if (entityFolderName != "Entities")
                {
                    queryFileNamespace += $"{entityFolderName}.";
                    sourceFileText = sourceFileText.Replace("{0}", $"{solutionName}.Application.Dtos.{entityFolderName}");
                }
                else
                {
                    sourceFileText = sourceFileText.Replace("{0}", $"{solutionName}.Application.Dtos");
                }

                queryFileNamespace += $"{entity.Name}Files.Queries";

                sourceFileText = sourceFileText.Replace("{1}", $"{queryFileNamespace}");
                sourceFileText = sourceFileText.Replace("{2}", $"{entity.Name}");

                sourceFileText = sourceFileText.Replace("{3}", pluralizer.Pluralize(entity.Name));

                var queryFilePath = System.IO.Path.Combine(queriesPath, $"GetAll{pluralizer.Pluralize(entity.Name)}Query.cs");
                System.IO.File.WriteAllText(queryFilePath, sourceFileText, System.Text.Encoding.UTF8);
            }
        }

        private static void CreateDtoAndReplaceMarkedValues(string dtosPath, Type entity, string dtoType, string solutionName, string entityFolderName)
        {
            if (File.Exists(Path.Combine(dtosPath, $"{entity.Name}{dtoType}Dto.cs")))
                return;

            if (DotNetCliHelper.RunCliCommand($"dotnet new class -n {entity.Name}{dtoType}Dto", dtosPath))
            {

                var sourceFilePath = System.IO.Path.Combine(Environment.CurrentDirectory, "TemplateFiles", "DtoTemplate.txt");

                var sourceFileText = File.ReadAllText(sourceFilePath);

                var propertiesAndNamespaces = GetPropertiesAndNamespaces(entity);

                var strUsings = new StringBuilder();
                foreach (var item in propertiesAndNamespaces.namespaces.Distinct())
                {
                    if (string.IsNullOrEmpty(item))
                        continue;

                    strUsings.AppendLine($"using {item};");
                }

                if (entityFolderName != "Entities")
                {
                    strUsings.AppendLine($"using {solutionName}.Application.Dtos;");
                    sourceFileText = sourceFileText.Replace("{0}", strUsings.ToString());

                    sourceFileText = sourceFileText.Replace("{1}", $"{solutionName}.Application.Dtos.{entityFolderName}");
                }
                else
                {
                    sourceFileText = sourceFileText.Replace("{0}", strUsings.ToString());
                    sourceFileText = sourceFileText.Replace("{1}", $"{solutionName}.Application.Dtos");
                }

                sourceFileText = sourceFileText.Replace("{2}", $"{entity.Name}{dtoType}Dto");

                if (dtoType != "Search")
                {
                    sourceFileText = sourceFileText.Replace("{3}", propertiesAndNamespaces.props);
                    sourceFileText = sourceFileText.Replace("{4}", "BaseDto");
                }
                else
                {
                    sourceFileText = sourceFileText.Replace("{3}", "");
                    sourceFileText = sourceFileText.Replace("{4}", "BaseSearchDto");
                }

                var dtoFilePath = System.IO.Path.Combine(dtosPath, $"{entity.Name}{dtoType}Dto.cs");
                System.IO.File.WriteAllText(dtoFilePath, sourceFileText, System.Text.Encoding.UTF8);

            }
        }

        private static void CreateValidatorAndReplaceMarkedValues(string validatorPath, Type entity, string dtoType, string solutionName, string entityFolderName)
        {
            if (File.Exists(Path.Combine(validatorPath, $"{entity.Name}{dtoType}DtoValidator.cs")))
                return; 

            if (DotNetCliHelper.RunCliCommand($"dotnet new class -n {entity.Name}{dtoType}DtoValidator", validatorPath))
            {

                var sourceFilePath = System.IO.Path.Combine(Environment.CurrentDirectory, "TemplateFiles", "ValidatorTemplate.txt");

                var sourceFileText = File.ReadAllText(sourceFilePath);

                if (entityFolderName != "Entities")
                {
                    sourceFileText = sourceFileText.Replace("{0}", $"{solutionName}.Application.Dtos.{entityFolderName}.Validators");
                }
                else
                {
                    sourceFileText = sourceFileText.Replace("{0}", $"{solutionName}.Application.Dtos.Validators");
                }

                sourceFileText = sourceFileText.Replace("{1}", entity.Name);
                sourceFileText = sourceFileText.Replace("{2}", dtoType);

                var dtoFilePath = System.IO.Path.Combine(validatorPath, $"{entity.Name}{dtoType}DtoValidator.cs");
                System.IO.File.WriteAllText(dtoFilePath, sourceFileText, System.Text.Encoding.UTF8);
            }
        }

        private static (string props, List<string> namespaces) GetPropertiesAndNamespaces(Type entity)
        {
            var strProperties = new StringBuilder();
            var namespaces = new List<string>();

            List<string> excludedProperties = new List<string>()
            {
                nameof(BaseEntity.Id).ToLower(),
                nameof(BaseEntity.CreatedDate).ToLower(),
                nameof(BaseEntity.ModifiedDate).ToLower(),
                nameof(BaseEntity.CreatedByUserId).ToLower(),
                nameof(BaseEntity.ModifiedByUserId).ToLower()
            };

            var properties = entity.GetProperties()
                .Where(p =>
                {
                    if (!p.CanRead || !p.CanWrite)
                    {
                        return false;
                    }

                    if (p.PropertyType.BaseType == typeof(BaseEntity))
                    {
                        return false;
                    }

                    if (p.PropertyType.IsGenericType && p.PropertyType.GetGenericTypeDefinition() == typeof(ICollection<>))
                    {
                        return false;
                    }

                    return !excludedProperties.Contains(p.Name.ToLower());
                });

            foreach (var property in properties)
            {
                string propType = property.PropertyType.Name;
                if (TypeMap.ContainsKey(property.PropertyType))
                    propType = TypeMap[property.PropertyType];

                strProperties.AppendLine("\t\tpublic " + propType + " " + property.Name + " { get; set; }");
                if (property.PropertyType.IsEnum && !string.IsNullOrEmpty(property.PropertyType.Namespace))
                    namespaces.Add(property.PropertyType.Namespace);
            }

            return (strProperties.ToString(), namespaces);
        }

        private static (string root_Directory, 
            string solutionName, 
            string applicationLayer_Directory, 
            string apiLayer_Directory, 
            IEnumerable<Type> entities) 
            GetProjectDirectories(List<string> excludedEntities)
        {
            string root_Directory = new DirectoryInfo("../../../../").FullName;

            var path_parts = root_Directory.Split(@"\").Where(x => !string.IsNullOrEmpty(x)).ToArray();
            string solutionName = path_parts[path_parts.Length - 1];

            string applicationLayer_Directory = Path.Combine(root_Directory, $"{solutionName}.Application");
            string apiLayer_Directory = Path.Combine(root_Directory, $"{solutionName}.API");

            var entitiesAssembly = typeof(BaseEntity).Assembly;
            IEnumerable<Type> entities = entitiesAssembly.GetExportedTypes()
                .Where(c => c.IsClass && 
                            !c.IsAbstract && 
                            c.IsPublic && 
                            typeof(BaseEntity).IsAssignableFrom(c) && 
                            !excludedEntities.Contains(c.Name.ToLower()));

            return (root_Directory, solutionName, applicationLayer_Directory, apiLayer_Directory, entities);
        }

        private static readonly Dictionary<Type, string> TypeMap = new Dictionary<Type, string>()
        {
            { typeof(int), "int" },
            { typeof(short), "short" },
            { typeof(bool), "bool" },
            { typeof(double), "double" },
            { typeof(long), "long" },
            { typeof(float), "float" },
            { typeof(decimal), "decimal" },
            { typeof(char), "char" }, // Remove quotes for strings (assuming "regular" format)

            { typeof(string), "string?" },

            { typeof(short?), "short?" },
            { typeof(int?), "int?" },  // Nullable types map to the underlying type
            { typeof(bool?), "bool?" },
            { typeof(double?), "double?" },
            { typeof(float?), "float?" },
            { typeof(decimal?), "decimal?" },
            { typeof(char?), "char?" },
            { typeof(long?), "long?" }, 
        };

        private static bool IsNullable(PropertyInfo property)
        {
            return Nullable.GetUnderlyingType(property.PropertyType) != null;
        }
    }
}
