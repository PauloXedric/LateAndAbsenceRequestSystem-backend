﻿namespace DLARS.Models.UserAccountModels
{
    public class ResetTokenValidationModel
    {
        public string Email { get; set; } = string.Empty;
        public string Token { get; set; } = string.Empty;
    }
}
