namespace MovieLog.DTOs;

public record UserDto
(
    int Id,
    string Username,
    string Email,
    string RoleName //numele rolului, entitatea rol
);
