using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Data;
using System.Data.SqlClient;
using System.Threading.Tasks;
using EcoSyncPickUpAPI.data;

namespace EcoSyncPickUpAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthenticationController : ControllerBase
    {
        // SignUp Endpoint
        [HttpPost("SignUp")]
        public async Task<IActionResult> SignUp([FromBody] CreateUserRequest request)
        {
            try
            {
                using (SqlConnection connection = new SqlConnection(SQLDB.GetConnectionString()))
                {
                    await connection.OpenAsync();

                    using (SqlCommand command = new SqlCommand("sp_CreateUser", connection))
                    {
                        command.CommandType = CommandType.StoredProcedure;

                        // Use the provided User ID from the request
                        command.Parameters.AddWithValue("@id", request.Id);  // Use the ID passed from the client
                        command.Parameters.AddWithValue("@name", request.Name);
                        command.Parameters.AddWithValue("@email", request.Email);
                        command.Parameters.AddWithValue("@password_hash", request.PasswordHash);
                        command.Parameters.AddWithValue("@role", string.IsNullOrEmpty(request.Role) ? "user" : request.Role);  // Default role to 'user' if not provided
                        command.Parameters.AddWithValue("@profile_picture", string.IsNullOrEmpty(request.ProfilePicture) ? "none" : request.ProfilePicture);  // Default to 'none' if empty

                        // Execute the command
                        var result = await command.ExecuteScalarAsync();
                        if (result != null && result.ToString() == "User created successfully")
                        {
                            return Ok(new
                            {
                                Status = "Success",
                                Message = "User created successfully.",
                                UserId = request.Id  // Return the provided User ID
                            });
                        }
                        else
                        {
                            return BadRequest(new
                            {
                                Status = "error",
                                Message = "Failed to create user."
                            });
                        }
                    }
                }
            }
            catch (SqlException ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, $"SQL Error: {ex.Message}");
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, $"Error: {ex.Message}");
            }
        }

        // Login Endpoint
        [HttpPost("Login")]
        public async Task<IActionResult> Login([FromBody] LoginRequest request)
        {
            try
            {
                using (SqlConnection connection = new SqlConnection(SQLDB.GetConnectionString()))
                {
                    await connection.OpenAsync();

                    using (SqlCommand command = new SqlCommand("sp_UserLogin", connection))
                    {
                        command.CommandType = CommandType.StoredProcedure;

                        command.Parameters.AddWithValue("@Email", request.Email);
                        command.Parameters.AddWithValue("@PasswordHash", request.PasswordHash);

                        using (SqlDataReader reader = await command.ExecuteReaderAsync())
                        {
                            if (await reader.ReadAsync())
                            {
                                // If login is successful, return user details
                                return Ok(new
                                {
                                    status = "success",
                                    message = "Login successful",
                                    data = new
                                    {
                                        user = new
                                        {
                                            id = reader["id"].ToString(),
                                            name = reader["name"].ToString(),
                                            email = reader["email"].ToString(),
                                            role = reader["role"].ToString(),
                                            profile_picture = reader["profile_picture"].ToString(),
                                            created_at = Convert.ToDateTime(reader["created_at"]).ToString("o")
                                        }
                                    }
                                });
                            }
                            else
                            {
                                // If login fails, return an error message
                                return BadRequest(new
                                {
                                    status = "error",
                                    message = "Invalid email or password"
                                });
                            }
                        }
                    }
                }
            }
            catch (SqlException ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, $"SQL Error: {ex.Message}");
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, $"Error: {ex.Message}");
            }
        }
    }
}
