﻿namespace App.API.Auth
{
    public class TokenOptions
    {
        public string Issuer { get; set; } = string.Empty;
        public string Audience { get; set; } = string.Empty;
        public string Secret { get; set; } = string.Empty;
        public int AccessTokenMinutes { get; set; } = 60;
    }
}