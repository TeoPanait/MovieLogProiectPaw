using System.ComponentModel.DataAnnotations;

namespace MovieLog.DTOs;

public record CreateUserDto
(
    [Required] string Username,
    [Required][EmailAddress] string Email, //validare automata pt format email
    [Required] string Password, //parola bruta criptata ulterior
    [Required] int RoleId //atribuim un id de rol
);
