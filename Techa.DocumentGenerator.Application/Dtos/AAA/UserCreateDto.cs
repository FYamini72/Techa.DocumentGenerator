﻿using Microsoft.AspNetCore.Http;

namespace Techa.DocumentGenerator.Application.Dtos.AAA
{
    public class UserCreateDto : BaseDto
    {
        public string UserName { get; set; }
        public string Password { get; set; }

        public string FirstName { get; set; }
        public string LastName { get; set; }

        public string? Email { get; set; }
        public string? Mobile { get; set; }
        public string? NationalCode { get; set; }

        public IFormFile? SelectedFile { get; set; }
    }
}