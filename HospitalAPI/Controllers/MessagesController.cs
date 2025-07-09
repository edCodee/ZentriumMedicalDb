using APIhospital.Data;
using Azure.Messaging;
using HospitalAPI.DTOs.ConversationHistoryDTO;
using Microsoft.AspNetCore.Mvc;
// Add the following using directive to resolve SqlConnection
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using System.Data.SqlClient;

namespace HospitalAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class MessagesController : ControllerBase
    {
        private readonly AppDbContext _context;
        public MessagesController(AppDbContext context)
        {
            _context = context;
        }


        private readonly string connectionString = "Server=DESKTOP-RC2HO3L;Database=ZentriumMedicalDB;Trusted_Connection=True;TrustServerCertificate=True;Connection Timeout=30;";

        [HttpPost]
        public IActionResult ReceiveMessage([FromBody] MessageDto message)
        {
            var userNumber = message.From;
            //var userMessage = message.Text.ToLower().Trim();
            var userMessage = message.Text?.ToLower().Trim(); // El texto puede ser nulo si es una imagen
            var incomingImageBase64 = message.ImageBase64Data;

            string state = GetConversationState(userNumber);
            string botResponse = "";

            switch (state)
            {
                case "initial":
                    botResponse = $"👋 ¡Hola! Gracias por escribirnos. ¿Te gustaría conocer nuestros productos disponibles hoy? 🩺";
                    SetConversationState(userNumber, "waiting_product_list");
                    break;

                case "waiting_product_list":
                    if (userMessage.Contains("si") || userMessage.Contains("claro") || userMessage.Contains("ver"))
                    {
                        botResponse = "🛒 *Productos disponibles:*\n" +
                            "1️⃣ Paracetamol 100mg\n" +
                            "2️⃣ Paracetamol 500mg\n" +
                            "3️⃣ Paracetamol 1000mg\n" +
                            "4️⃣ Ibuprofeno 400mg\n" +
                            "5️⃣ Aspirina 100mg\n\n" +
                            "Por favor indícame el *número* o el *nombre* del producto que deseas. 😊";
                        SetConversationState(userNumber, "waiting_product_choice");
                    }
                    else
                    {
                        botResponse = "👌 Sin problema. Si necesitas otra cosa, solo escribe *ayuda* o *menú*.";
                        SetConversationState(userNumber, "initial");
                    }
                    break;

                case "waiting_product_choice":
                    {
                        string selectedProduct = "";
                        switch (userMessage)
                        {
                            case "1":
                            case "paracetamol 100":
                            case "paracetamol 100mg":
                                selectedProduct = "Paracetamol 100mg";
                                break;
                            case "2":
                            case "paracetamol 500":
                            case "paracetamol 500mg":
                                selectedProduct = "Paracetamol 500mg";
                                break;
                            case "3":
                            case "paracetamol 1000":
                            case "paracetamol 1000mg":
                                selectedProduct = "Paracetamol 1000mg";
                                break;
                            case "4":
                            case "ibuprofeno":
                            case "ibuprofeno 400":
                                selectedProduct = "Ibuprofeno 400mg";
                                break;
                            case "5":
                            case "aspirina":
                            case "aspirina 100":
                                selectedProduct = "Aspirina 100mg";
                                break;
                        }

                        if (!string.IsNullOrEmpty(selectedProduct))
                        {
                            botResponse = $"✅ Has elegido *{selectedProduct}*.\n" +
                                "🚚 ¿Prefieres *retiro en tienda* o *envío a domicilio*? (envío con costo adicional de $2) 🏠";
                            // aquí podrías también guardar el producto elegido en otra tabla
                            SetConversationState(userNumber, "waiting_delivery_option");
                        }
                        else
                        {
                            botResponse = "🤔 No logré entender el producto, por favor indica el *número* o el *nombre* completo.";
                        }
                        break;
                    }

                case "waiting_delivery_option":
                    if (userMessage.Contains("domicilio"))
                    {
                        botResponse = "🏠 Perfecto, te lo enviaremos a domicilio con un costo adicional de $2.\n" +
                                      "💳 ¿Cómo deseas pagar? *Efectivo* o *Transferencia*.";
                        SetConversationState(userNumber, "waiting_payment");
                    }
                    else if (userMessage.Contains("retiro"))
                    {
                        botResponse = "🛍️ Genial, pasas a retirarlo en tienda.\n" +
                                      "💳 ¿Cómo deseas pagar? *Efectivo* o *Transferencia*.";
                        SetConversationState(userNumber, "waiting_payment");
                    }
                    else
                    {
                        botResponse = "😅 No entendí, ¿prefieres *retiro* en tienda o *domicilio*?";
                    }
                    break;

                case "waiting_payment":
                    if (userMessage.Contains("efectivo"))
                    {
                        botResponse = "💵 Perfecto, hemos registrado tu pedido para pago en efectivo.\n" +
                                      "✅ ¡Gracias por tu compra! Pronto nos contactaremos para coordinar la entrega.";
                        SetConversationState(userNumber, "initial");
                    }
                    else if (userMessage.Contains("transferencia"))
                    {
                        botResponse = "💳 Perfecto, hemos registrado tu pedido para pago por transferencia.\n" +
                                      "✅ ¡Gracias por tu compra! Te enviaremos los datos bancarios para realizar el pago.";
                        SetConversationState(userNumber, "initial");
                    }
                    else
                    {
                        botResponse = "😅 No logré entender tu forma de pago, ¿será *efectivo* o *transferencia*?";
                    }
                    break;

                default:
                    botResponse = "👋 ¡Hola! Soy el asistente virtual de Zentrium Medical, ¿en qué podemos ayudarte hoy?";
                    SetConversationState(userNumber, "initial");
                    break;
            }

            // Llama a SaveHistory con la imagen recibida
            SaveHistory(userNumber, userMessage, botResponse, state, incomingImageBase64);





            return Ok(new { text = botResponse });
        }




        //[HttpGet]
        //public async Task<ActionResult<IEnumerable<ConversationHistoryReadDTO>>> GetConversation()
        //{
        //    var mess = await _context.ConversationHistory.ToListAsync();

        //    var messDTO = mess.Select(f => new ConversationHistoryReadDTO
        //    {
        //        MessId = f.Id,
        //        MessUserNumber = f.UserNumber,
        //        MessageTex = f.MessageText,
        //        MessBootResponse = f.BotResponse,
        //        MessConversationState = f.ConversationState,
        //        MessCreatedAt = f.CreatedAt,
        //        MessImage = f.ImageBase64 // ✅ ESTA LÍNEA FALTABA
        //    });

        //    return Ok(messDTO);
        //}

        //[HttpGet]
        //public async Task<ActionResult<IEnumerable<ConversationHistoryReadDTO>>> GetConversation()
        //{
        //    var mess = await _context.ConversationHistory.ToListAsync();

        //    var messDTO = mess.Select(f => new ConversationHistoryReadDTO
        //    {
        //        MessId = f.Id,
        //        MessUserNumber = f.UserNumber,
        //        MessageTex = f.MessageText,         // Si `f.MessageText` contiene el texto del mensaje
        //        MessBootResponse = f.BotResponse,
        //        MessConversationState = f.ConversationState,
        //        MessCreatedAt = f.CreatedAt,
        //        MessImage = f.ImageBase64           // <-- ¡Aquí está! Mapea la Base64 de la entidad a tu DTO
        //    });

        //    return Ok(messDTO);
        //}
        // Tu Backend (C#) - Archivo del controlador (MessagesController.cs o similar)

        // ... (tus using statements, etc.) ...

        [HttpGet]
        public async Task<ActionResult<IEnumerable<ConversationHistoryReadDTO>>> GetConversation()
        {
            var mess = await _context.ConversationHistory.ToListAsync();

            var messDTO = mess.Select(f =>
            {
                // Esta es la lógica crucial para decidir si el contenido de MessageText es una imagen.
                // Nos basamos en la firma de inicio de JPEG (/9j/) y que sea una cadena de longitud considerable.
                bool isMessageTextAnImage = !string.IsNullOrEmpty(f.MessageText) &&
                                             f.MessageText.StartsWith("/9j/") &&
                                             f.MessageText.Length > 100; // Un valor seguro para descartar textos cortos.

                return new ConversationHistoryReadDTO
                {
                    MessId = f.Id,
                    MessUserNumber = f.UserNumber,
                    MessBootResponse = f.BotResponse,
                    MessConversationState = f.ConversationState,
                    MessCreatedAt = f.CreatedAt,

                    // Lógica de mapeo condicional:
                    // Si detectamos que MessageText es una imagen Base64, asignamos eso a MessImage
                    // y ponemos MessageTex como null.
                    MessImage = isMessageTextAnImage ? f.MessageText : null,

                    // Si MessageText NO es una imagen Base64, entonces es el texto del mensaje,
                    // así que lo asignamos a MessageTex.
                    MessageTex = isMessageTextAnImage ? null : f.MessageText
                };
            });

            return Ok(messDTO);
        }


        private void SaveHistory(string user, string userMessage, string botResponse, string state, string? imageBase64)
        {
            using var connection = new SqlConnection(connectionString);
            connection.Open();

            var cmd = connection.CreateCommand();
            cmd.CommandText = @"
        INSERT INTO ConversationHistory
        (UserNumber, MessageText, BotResponse, ConversationState, CreatedAt, ImageBase64)
        VALUES (@user, @msg, @bot, @state, GETDATE(), @img)";

            cmd.Parameters.AddWithValue("@user", user);
            cmd.Parameters.AddWithValue("@msg", (object?)userMessage ?? DBNull.Value);
            cmd.Parameters.AddWithValue("@bot", botResponse);
            cmd.Parameters.AddWithValue("@state", state);
            cmd.Parameters.AddWithValue("@img", (object?)imageBase64 ?? DBNull.Value);

            cmd.ExecuteNonQuery();
        }


        private string GetConversationState(string user)
        {
            using var connection = new SqlConnection(connectionString);
            connection.Open();
            var cmd = connection.CreateCommand();
            cmd.CommandText = "SELECT TOP 1 ConversationState FROM ConversationHistory WHERE UserNumber = @user ORDER BY CreatedAt DESC";
            cmd.Parameters.AddWithValue("@user", user);
            var result = cmd.ExecuteScalar();
            return result?.ToString() ?? "initial";
        }

        private void SetConversationState(string user, string state)
        {
            // podría guardarse en la tabla ConversationHistory como un nuevo registro
            // o en otra tabla de usuarios activos
            using var connection = new SqlConnection(connectionString);
            connection.Open();
            var cmd = connection.CreateCommand();
            cmd.CommandText = "INSERT INTO ConversationHistory (UserNumber, MessageText, BotResponse, ConversationState) VALUES (@user, '', '', @state)";
            cmd.Parameters.AddWithValue("@user", user);
            cmd.Parameters.AddWithValue("@state", state);
            cmd.ExecuteNonQuery();
        }
    }


    public class MessageDto
    {
        public string From { get; set; } = string.Empty;
        public string Text { get; set; } = string.Empty;
        public string? ImageBase64Data { get; set; } // Nuevo campo para la imagen Base64

    }
}
