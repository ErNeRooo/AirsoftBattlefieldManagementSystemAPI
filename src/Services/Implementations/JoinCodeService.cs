using AirsoftBattlefieldManagementSystemAPI.Models.Entities;
using AirsoftBattlefieldManagementSystemAPI.Services.Abstractions;
using System;
using AirsoftBattlefieldManagementSystemAPI.Enums;
using AirsoftBattlefieldManagementSystemAPI.Exceptions;

namespace AirsoftBattlefieldManagementSystemAPI.Services.Implementations
{
    public class JoinCodeService(IBattleManagementSystemDbContext dbContext) : IJoinCodeService
    {
        public string Generate(JoinCodeFormat joinCodeFormat, int codeLength)
        {
            switch (joinCodeFormat)
            {
                case JoinCodeFormat.From0to9:
                    return GenerateWithCustomCharacters(codeLength);
                case JoinCodeFormat.From0to9andAtoZ:
                    return GenerateWithCustomCharacters(codeLength);
                default:
                    return GenerateWithCustomCharacters(codeLength);
            }
        }

        private string GenerateWithCustomCharacters(int codeLength)
        {
            char[] charsArray = new char[codeLength];
            Array.Fill(charsArray, '0');

            while (new string(charsArray).All(c => c == 'Z'));
            {
                for (int j = 48; j <= 90; j++)
                {
                    if (j == ':')
                    {
                        j = 'A';
                    }

                    charsArray[charsArray.Length - 1] = (char)j;

                    bool isJoinCodeAvailable = 0 == dbContext.Room.Count(r => r.JoinCode == new string(charsArray));

                    if (isJoinCodeAvailable) return new string(charsArray);
                }

                for (int j = 1; j <= charsArray.Length; j++)
                {
                    char c = (char)(charsArray[charsArray.Length - j] + 1);

                    if (c == ':')
                    {
                        c = 'A';
                    }

                    if (c == 'Z' + 1)
                    {
                        continue;
                    }

                    charsArray[charsArray.Length - j] = c;

                    for (int k = j - 1; k >= 1; k--)
                    {
                        charsArray[charsArray.Length - k] = '0';
                    }

                    break;
                }
            }

            throw new NotASingleAvailableJoinCodeException("There's not a single available join number");
        }
    }
}
