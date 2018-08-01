using System;
using System.Collections.Generic;
using Foundation;
using LetterApp.Core.Models;
using LetterApp.Core.Models.Generic;
using LetterApp.iOS.Helpers;
using LetterApp.iOS.Views.CustomViews.Cells;
using LetterApp.iOS.Views.UserSettings.Cells;
using UIKit;

namespace LetterApp.iOS.Sources
{
    public class UserSettingsSource : UITableViewSource
    {
        private SettingsPhoneModel _phoneModel;
        private SettingsAllowCallsModel _allowCallsModel;
        private DescriptionTypeEventModel _passwordTypeModel;
        private List<DescriptionTypeEventModel> _typeModelInformation;
        private List<DescriptionTypeEventModel> _typeModelDanger;
        private List<DescriptionAndBoolEventModel> _switchModel;
        private Dictionary<string, string> _locationResources;

        public UserSettingsSource(UITableView tableView, SettingsPhoneModel phoneModel, SettingsAllowCallsModel allowCallsModel, 
                                  DescriptionTypeEventModel passwordTypeModel, List<DescriptionAndBoolEventModel> switchModel, 
                                  List<DescriptionTypeEventModel> typeModelInformation, List<DescriptionTypeEventModel> typeModelDanger, 
                                  Dictionary<string, string> locationResources)
        {
            _phoneModel = phoneModel;
            _allowCallsModel = allowCallsModel;
            _passwordTypeModel = passwordTypeModel;
            _typeModelInformation = typeModelInformation;
            _typeModelDanger = typeModelDanger;
            _switchModel = switchModel;
            _locationResources = locationResources;

            tableView.RegisterNibForCellReuse(LabelWithArrowCell.Nib, LabelWithArrowCell.Key);
            tableView.RegisterNibForCellReuse(SwitchCell.Nib, SwitchCell.Key);
            tableView.RegisterNibForCellReuse(AllowPhoneCallsCell.Nib, AllowPhoneCallsCell.Key);
            tableView.RegisterNibForCellReuse(PhoneNumberCell.Nib, PhoneNumberCell.Key);
        }

        public override string TitleForHeader(UITableView tableView, nint section)
        {
            switch (section)
            {
                case (int)Sections.Account: return _locationResources["account"];
                case (int)Sections.Notifications: return _locationResources["notifications"];
                case (int)Sections.Information: return _locationResources["information"];
                case (int)Sections.DangerZone: return _locationResources["dangerzone"];
                default: return string.Empty;
            }
        }

        public override nfloat GetHeightForHeader(UITableView tableView, nint section) => LocalConstants.Settings_Sections;

        public override UITableViewCell GetCell(UITableView tableView, NSIndexPath indexPath)
        {
            var cell = new UITableViewCell();

            switch (indexPath.Section)
            {
                case (int)Sections.Account:
                    switch (indexPath.Row)
                    {
                        case (int)Account.PhoneNumber: 
                            var phoneCell = tableView.DequeueReusableCell(PhoneNumberCell.Key) as PhoneNumberCell;
                            phoneCell.SelectionStyle = UITableViewCellSelectionStyle.None;
                            phoneCell.Configure(_phoneModel);
                            cell = phoneCell;
                            break;
                        case (int)Account.AllowPhoneCalls:
                            var allowCallsCell = tableView.DequeueReusableCell(AllowPhoneCallsCell.Key) as AllowPhoneCallsCell;
                            allowCallsCell.SelectionStyle = UITableViewCellSelectionStyle.None;
                            allowCallsCell.Configure(_allowCallsModel);
                            cell = allowCallsCell;
                            break;
                        case (int)Account.Password:
                            var passwordCell = tableView.DequeueReusableCell(LabelWithArrowCell.Key) as LabelWithArrowCell;
                            passwordCell.Configure(_passwordTypeModel);
                            cell = passwordCell;
                            break;
                    }
                    break;

                case (int)Sections.Notifications:
                    var notificationCell = tableView.DequeueReusableCell(SwitchCell.Key) as SwitchCell;
                    notificationCell.SelectionStyle = UITableViewCellSelectionStyle.None;
                    notificationCell.Configure(_switchModel[indexPath.Row]);
                    cell = notificationCell;
                    break;
                case (int)Sections.Information:
                    var infoCell = tableView.DequeueReusableCell(LabelWithArrowCell.Key) as LabelWithArrowCell;
                    infoCell.Configure(_typeModelInformation[indexPath.Row]);
                    cell = infoCell;
                    break;

                case (int)Sections.DangerZone:
                    var dangerCell = tableView.DequeueReusableCell(LabelWithArrowCell.Key) as LabelWithArrowCell;
                    dangerCell.Configure(_typeModelDanger[indexPath.Row]);
                    cell = dangerCell;
                    break;
            }

            return cell;
        }

        public override void RowSelected(UITableView tableView, NSIndexPath indexPath)
        {
            tableView.DeselectRow(indexPath, true);

            if (indexPath.Section == (int)Sections.Account)
            {
                if(indexPath.Row == (int)Account.Password)
                {
                    _passwordTypeModel?.TypeEvent?.Invoke(this, _passwordTypeModel.Type);
                }
            }
            else if(indexPath.Section == (int)Sections.Information)
            {
                _typeModelInformation[indexPath.Row]?.TypeEvent?.Invoke(this, _typeModelInformation[indexPath.Row].Type);
            }
            else if (indexPath.Section == (int)Sections.DangerZone)
            {
                _typeModelDanger[indexPath.Row]?.TypeEvent?.Invoke(this, _typeModelDanger[indexPath.Row].Type);
            }
        }

        public override nfloat GetHeightForRow(UITableView tableView, NSIndexPath indexPath)
        {
            switch (indexPath.Section)
            {
                case (int)Sections.Account: 
                {
                    switch (indexPath.Row)
                    {
                        case (int)Account.PhoneNumber: return LocalConstants.Settings_GenericCells;
                        case (int)Account.AllowPhoneCalls: return LocalConstants.Settings_AllowCalls;
                        case (int) Account.Password: return LocalConstants.Settings_GenericCells;
                        default: return 0;
                    }
                }
                case (int)Sections.Notifications: return LocalConstants.Settings_GenericCells;
                case (int)Sections.Information: return LocalConstants.Settings_GenericCells;
                case (int)Sections.DangerZone: return LocalConstants.Settings_GenericCells;
                default: return 0;
            }
        }

        public override nint RowsInSection(UITableView tableview, nint section)
        {
            switch (section)
            {
                case (int)Sections.Account: return (int)Account.Count;
                case (int)Sections.Notifications: return (int)Notifications.Count;
                case (int)Sections.Information: return (int)Information.Count;
                case (int)Sections.DangerZone: return (int)DangerZone.Count;                                 
                default: return 0;
            }
        }

        public override nint NumberOfSections(UITableView tableView) => (int)Sections.Count;

        private enum Sections
        {
            Account,
            Notifications,
            Information,
            DangerZone,
            Count
        }

        public enum Account
        {
            PhoneNumber,
            AllowPhoneCalls,
            Password,
            Count
        }

        public enum Notifications
        {
            MessageNotifications,
            CallNotifications,
            GroupNotifications,
            Count
        }

        public enum Information
        {
            ContactUs,
            TermsOfService,
            CreateOrganization,
            Count
        }

        public enum DangerZone
        {
            SignOut,
            LeaveDivision,
            LeaveOrganization,
            DeleteAccount,
            Count
        }
    }
}
