using System;
using System.Collections.Generic;
using LetterApp.Core.Localization;
using LetterApp.Core.Services.Interfaces;

namespace LetterApp.Core.Services
{
    public class CodeResultService : ICodeResultService
    {
        private Dictionary<int, string> CodeDictionary = new Dictionary<int, string>();

        public void SetCodes()
        {
            CodeDictionary.Add(101, Login_WrongPassword);
            CodeDictionary.Add(102, Login_AccountNotActive);
            CodeDictionary.Add(103, Login_AccountDesactivated);
            CodeDictionary.Add(104, Password_EmailNoMatch);
            CodeDictionary.Add(105, Password_ErrorChanging);
            CodeDictionary.Add(106, Password_VerificationCode);
            CodeDictionary.Add(107, User_NoDivisionMatch);
            CodeDictionary.Add(108, User_EmailDomain);
            CodeDictionary.Add(109, User_WrongPassword);
            CodeDictionary.Add(110, Registration_EmailInUse);
            CodeDictionary.Add(111, Registration_WrongCode);
            CodeDictionary.Add(112, Organization_NotFound);
            CodeDictionary.Add(113, Organization_EmailNotValid);
            CodeDictionary.Add(114, Organization_NoDivisions);
            CodeDictionary.Add(115, Organization_WrongCode);
            CodeDictionary.Add(116, Organization_NoPosition);

            CodeDictionary.Add(201, Password_Changed);
            CodeDictionary.Add(202, User_DivisionSet);
            CodeDictionary.Add(203, User_UpdatePosition);
            CodeDictionary.Add(204, User_Update);
            CodeDictionary.Add(205, User_Desactivate);
            CodeDictionary.Add(206, User_DivisionLeft);
        }

        public string GetCodeDescription(int code)
        {
            return CodeDictionary[code];
        }

        #region resources

        //errors
        string Login_WrongPassword => L10N.Localize("Code_E101");
        string Login_AccountNotActive => L10N.Localize("Code_E102");
        string Login_AccountDesactivated => L10N.Localize("Code_E103");
        string Password_EmailNoMatch => L10N.Localize("Code_E104");
        string Password_ErrorChanging => L10N.Localize("Code_E105");
        string Password_VerificationCode => L10N.Localize("Code_E106");
        string User_NoDivisionMatch => L10N.Localize("Code_E107");
        string User_EmailDomain => L10N.Localize("Code_E108");
        string User_WrongPassword => L10N.Localize("Code_E109");
        string Registration_EmailInUse => L10N.Localize("Code_E110");
        string Registration_WrongCode => L10N.Localize("Code_E111");
        string Organization_NotFound => L10N.Localize("Code_E112");
        string Organization_EmailNotValid => L10N.Localize("Code_E113");
        string Organization_NoDivisions => L10N.Localize("Code_E114");
        string Organization_WrongCode => L10N.Localize("Code_E115");
        string Organization_NoPosition => L10N.Localize("Code_E116");

        //success
        string Password_Changed => L10N.Localize("Code_S201");
        string User_DivisionSet => L10N.Localize("Code_S202");
        string User_UpdatePosition => L10N.Localize("Code_S203");
        string User_Update => L10N.Localize("Code_S204");
        string User_Desactivate => L10N.Localize("Code_S205");
        string User_DivisionLeft => L10N.Localize("Code_S206");

        #endregion
    }
}
