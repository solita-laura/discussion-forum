using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using DiscussionForum.Models;
using Microsoft.AspNetCore.Authentication;
using Microsoft.IdentityModel.Tokens;

namespace DiscussionForum.Services;
public class UserService
{
    private readonly UserContext _context;
    private readonly IConfiguration _configuration;

    public UserService (IConfiguration configuration, UserContext context){
        _configuration=configuration;
        _context = context;
    }

    /// <summary>
    /// Log the user in with the given username and password.
    /// Check if username is found and if the passwords match.
    /// If yes, return the jwt token.
    /// </summary>
    /// <param name="loginRequest">LoginRequest</param>
    /// <returns>Task<string></returns>
    /// <exception cref="Exception"></exception>

    public async Task<string> Login (LoginRequest loginRequest){

        try{
            var user = _context.users.FirstOrDefault(u => u.username.Equals(loginRequest.Username));

            if(user is null){
                throw new UserNotFoundException("No user found with the given username.");
            }

            byte[] salt = Convert.FromBase64String(user.salt);
            byte[] hashedPass = Convert.FromBase64String(user.password);

            var pass = await DecryptPassword(hashedPass, salt, loginRequest.Password);

            if(loginRequest.Password.Equals(pass))
            {
                var token = GenerateJwtToken(user.username);
                return token;
            }

            throw new InvalidPasswordException("Invalid password."); 

        }catch (UserNotFoundException ex){
            throw new UserNotFoundException(ex.Message); 
        }
        catch (InvalidPasswordException ex){
            throw new InvalidPasswordException(ex.Message);
        }
        catch (Exception ex){
            throw new Exception("Error logging in the user.");
        } 
    }

    /// <summary>
    /// Create a key for encrypting a password
    /// </summary>
    /// <param name="password">string</param>
    /// <param name="salt">byte array</param>
    /// <returns>byte array</returns>
    /// <exception cref="Exception"></exception>

    public byte[] CreateKey (string password, byte[] salt){
        try{
            var iterations = 1000;
            var desiredKeyLength = 16; 
            var hashMethod = HashAlgorithmName.SHA384;
            return Rfc2898DeriveBytes.Pbkdf2(Encoding.Unicode.GetBytes(password),
                                            salt,
                                            iterations,
                                            hashMethod,
                                            desiredKeyLength);
        }catch{
            Console.WriteLine("Error creating the key");
            throw new Exception("Error creating the key.");
        }
    }

    /// <summary>
    /// create a salt for encrypting a password
    /// </summary>
    /// <param name="length">integer</param>
    /// <returns>byte array</returns>
    /// <exception cref="Exception"></exception>
    public byte[] CreateSalt (int length){
        try{

            using var rng = new RNGCryptoServiceProvider();
            byte [] randomBytes = new byte[length];
            rng.GetBytes(randomBytes);

            return randomBytes;
        }catch{
            Console.WriteLine("Error creating the salt.");
            throw new Exception("Error creating the salt.");
        }
    }
    /// <summary>
    /// Encrypt a password before storing
    /// </summary>
    /// <param name="password">string</param>
    /// <param name="salt">byte array</param>
    /// <returns>byte array</returns>
    /// <exception cref="Exception"></exception>

    public async Task<byte[]> EncryptPassword (string password, byte[] salt){
        try{
            using Aes aes = Aes.Create();
            aes.Key = CreateKey(password, salt);
            aes.IV = CreateSalt(16);

            using MemoryStream output = new();
            using (CryptoStream cryptoStream = new(output, aes.CreateEncryptor(), CryptoStreamMode.Write)){
                await cryptoStream.WriteAsync(salt);
                await cryptoStream.WriteAsync(aes.IV);
                byte[] passwordBytes = Encoding.Unicode.GetBytes(password);
                await cryptoStream.WriteAsync(passwordBytes, 0, passwordBytes.Length);
                await cryptoStream.FlushFinalBlockAsync();
            }

            return output.ToArray();

        }catch{
            Console.WriteLine("Error encrypting the password");
            throw new Exception("Error encrypting the password");
        }
    }

    /// <summary>
    /// Decrypt a password for logging in
    /// </summary>
    /// <param name="encryptedPassword">byte array</param>
    /// <param name="salt">byte array</param>
    /// <param name="password">string</param>
    /// <returns>string</returns>
    public async Task<string> DecryptPassword (byte[] encryptedPassword, byte[] salt, string password){

        try{
            using Aes aes = Aes.Create();
            aes.Key = CreateKey(password, salt);

            byte[] iv = new byte[16];

            Array.Copy(encryptedPassword, 16, iv, 0, 16);

            aes.IV = iv;

            using MemoryStream input = new(encryptedPassword[32..]);
            using CryptoStream cryptoStream = new(input, aes.CreateDecryptor(), CryptoStreamMode.Read);
            using MemoryStream output = new();

            await cryptoStream.CopyToAsync(output);

            return Encoding.Unicode.GetString(output.ToArray());
        }catch{
            Console.WriteLine("Error decrypting a password.");
            throw new InvalidPasswordException("Invalid password.");
        }
    }

    /// <summary>
    /// Generate a jwt token once user is authenticated
    /// </summary>
    /// <param name="username">string</param>
    /// <returns>string</returns>
    private string GenerateJwtToken(string username)
    {
        try{
            var claims = new []
                {
                    new Claim(JwtRegisteredClaimNames.Sub, username),
                    new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
                };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Secret"]));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                issuer: _configuration["Jwt:ValidIssuer"],
                audience: _configuration["Jwt:ValidAudience"],
                claims: claims,
                expires: DateTime.Now.AddMinutes(30),
                signingCredentials: creds
                );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }catch{
            throw new Exception("Error creating the JWT token");
        }
    }

}
