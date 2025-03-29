using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Galini.Models.Payload;
using Galini.Models.Payload.Request.Bank;
using Galini.Services.Interface;
using QRCoder;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Drawing.Processing;
using SixLabors.ImageSharp.Formats;
using SixLabors.ImageSharp.Formats.Png;
using SixLabors.ImageSharp.PixelFormats;
using SixLabors.ImageSharp.Processing;


namespace Galini.Services.Implement
{
    public class BankService : IBankService
    {
        public string GeneratePaymentQRCode(BankRequest request)
        {
            string bankCode = GetBankCode(request.BankName);
            if (string.IsNullOrEmpty(bankCode))
            {
                throw new ArgumentException("Ngân hàng không hợp lệ.");
            }

            Console.WriteLine($"Bank Code: {bankCode}");
            Console.WriteLine($"Account Number: {request.AccountNumber}");
            Console.WriteLine($"Amount: {request.Amount}");
            Console.WriteLine($"Description: {request.Description}");

            string payload = $"000201010211" +
                             $"38{(bankCode.Length + 6):D2}0108{bankCode}" +
                             $"02{request.AccountNumber.Length:D2}{request.AccountNumber}" + 
                             $"5303704" +  
                             $"54{request.Amount:00}" +  
                             $"58VN" + 
                             $"62{Encoding.UTF8.GetByteCount(request.Description):D2}{request.Description}";  

            string crc = GenerateCRC16(payload + "6304");
            string qrContent = payload + "6304" + crc;

            Console.WriteLine("QR Content: " + qrContent);

            using (QRCodeGenerator qrGenerator = new QRCodeGenerator())
            {
                QRCodeData qrCodeData = qrGenerator.CreateQrCode(qrContent, QRCodeGenerator.ECCLevel.Q);
                PngByteQRCode qrCode = new PngByteQRCode(qrCodeData);

                byte[] qrCodeBytes = qrCode.GetGraphic(8);
                string fileName = $"payment_qr_{Guid.NewGuid()}.png";

                try
                {
                    File.WriteAllBytes(fileName, qrCodeBytes);
                    Console.WriteLine("QR Code saved: " + fileName);
                    return fileName;
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Error saving file: " + ex.Message);
                    return null;
                }
            }
        }

        private string GenerateCRC16(string input)
        {
            const ushort polynomial = 0x1021;
            ushort crc = 0xFFFF;
            byte[] bytes = Encoding.UTF8.GetBytes(input); 

            foreach (byte b in bytes)
            {
                crc ^= (ushort)(b << 8);
                for (int i = 0; i < 8; i++)
                {
                    if ((crc & 0x8000) != 0)
                        crc = (ushort)((crc << 1) ^ polynomial);
                    else
                        crc <<= 1;
                }
            }
            return $"{crc:X4}";
        }




        public Dictionary<string, string> GetAllBanks()
        {
            return BankData.BankCodes;
        }

        public string GetBankCode(string bankName)
        {
            if (BankData.BankCodes.TryGetValue(bankName, out string bankCode))
            {
                return bankCode;
            }
            return null;
        }

        public bool IsValidBank(string bankName)
        {
            return BankData.BankCodes.ContainsKey(bankName);
        }
    }
}
