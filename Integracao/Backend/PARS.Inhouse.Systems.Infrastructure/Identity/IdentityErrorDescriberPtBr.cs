using Microsoft.AspNetCore.Identity;

public class IdentityErrorDescriberPtBr : IdentityErrorDescriber
{
    public override IdentityError DuplicateUserName(string userName)
        => new IdentityError { Code = nameof(DuplicateUserName), Description = $"O nome de usuário '{userName}' já está em uso." };

    public override IdentityError DuplicateEmail(string email)
        => new IdentityError { Code = nameof(DuplicateEmail), Description = $"O e-mail '{email}' já está em uso." };

    public override IdentityError InvalidUserName(string userName)
        => new IdentityError { Code = nameof(InvalidUserName), Description = $"O nome de usuário '{userName}' é inválido." };

    public override IdentityError InvalidEmail(string email)
        => new IdentityError { Code = nameof(InvalidEmail), Description = $"O e-mail '{email}' é inválido." };

    public override IdentityError PasswordTooShort(int length)
        => new IdentityError { Code = nameof(PasswordTooShort), Description = $"A senha deve conter pelo menos {length} caracteres." };

    public override IdentityError PasswordRequiresDigit()
        => new IdentityError { Code = nameof(PasswordRequiresDigit), Description = "A senha deve conter pelo menos um número." };

    public override IdentityError PasswordRequiresUpper()
        => new IdentityError { Code = nameof(PasswordRequiresUpper), Description = "A senha deve conter pelo menos uma letra maiúscula." };

    public override IdentityError PasswordRequiresLower()
        => new IdentityError { Code = nameof(PasswordRequiresLower), Description = "A senha deve conter pelo menos uma letra minúscula." };

    public override IdentityError PasswordRequiresNonAlphanumeric()
        => new IdentityError { Code = nameof(PasswordRequiresNonAlphanumeric), Description = "A senha deve conter pelo menos um caractere especial." };
}