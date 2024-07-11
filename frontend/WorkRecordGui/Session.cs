using System.ComponentModel;
using System.IdentityModel.Tokens.Jwt;
using System.Runtime.CompilerServices;
using System.Security.Claims;
using WorkRecordGui.Models;
using WorkRecordGui.Shared.Dtos.User;

namespace WorkRecordGui
{
    public class Session : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler? PropertyChanged;
        private string _token = "";
        public string Token
        {
            get => _token;
            set
            {
                _token = value;
                DecodeToken();
                OnPropertyChanged(nameof(Token));
                OnPropertyChanged(nameof(Role));
                OnPropertyChanged(nameof(IsUser));
                OnPropertyChanged(nameof(IsUserCoordinator));
                OnPropertyChanged(nameof(IsUserManager));
                OnPropertyChanged(nameof(IsUserAdmin));
                OnPropertyChanged(nameof(IsUserEmployee));
            }
        
        }
        private Role _role = Role.user;
        public Role Role
        {
            get
            {
                return _role;
            }
            set
            {
                _role = value;
            }
        }
        public int UserId { get; set; } = -1;

        private GetUserDto _user = new();
        public GetUserDto User
        {
            get => _user;
            set
            {
                _user = value;
                OnPropertyChanged();
            }
        }

        private void DecodeToken()
        {
            if (string.IsNullOrEmpty(_token))
            {
                return;
            }

            var handler = new JwtSecurityTokenHandler();
            var jwtToken = handler.ReadJwtToken(_token);

            var roleClaim = jwtToken.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Role);
            if (roleClaim is not null)
            {
                Role = (Role)int.Parse(roleClaim.Value);
            }

            var userIdClaim = jwtToken.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier);
            if (userIdClaim is not null)
            {
                UserId = int.Parse(userIdClaim.Value);
            }
        }

        public Session()
        {
            Token = "";
        }

        public bool IsUser => Role >= Role.user;
        public bool IsUserCoordinator => Role >= Role.coordinator;
        public bool IsUserManager => Role >= Role.manager;
        public bool IsUserAdmin => Role >= Role.admin;
        public bool IsUserEmployee => User.EmployeeId is not null;

        protected void OnPropertyChanged([CallerMemberName] string? propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
