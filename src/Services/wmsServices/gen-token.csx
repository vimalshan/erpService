using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.IdentityModel.Tokens;

var key = "ShipmentServiceSuperSecretKeyThatIsAtLeast32CharactersLong!";
var secKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(key));
var creds = new SigningCredentials(secKey, SecurityAlgorithms.HmacSha256);
var claims = new[] {
    new Claim("sub", "1"),
    new Claim("unique_name", "admin"),
    new Claim(ClaimTypes.Role, "Admin"),
    new Claim(ClaimTypes.Role, "Warehouse")
};
var token = new JwtSecurityToken("ShipmentService", "ShipmentServiceClients", claims, expires: DateTime.UtcNow.AddHours(2), signingCredentials: creds);
Console.WriteLine(new JwtSecurityTokenHandler().WriteToken(token));
