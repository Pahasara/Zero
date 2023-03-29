// Copyright (c) 2023 Dewnith Fernando @github.com/Pahasara.
// Licensed under the MIT license.

namespace Zero.Core
{
    public class Message
    {
        public string DeleteConfirmTitle = "Confirm Delete";

        public string DeleteConfirmMessage = "Are you sure want to permanently delete this show?";

        public string ResetTitle = "Confirm Reset";
        
        public string ResetMessage = "Are you sure want to reset the progress of this show?";

        public string DatabaseLostTitle = "DB Not Found";

        public string DatabaseLostMessage = "'SQL Local DB' is not installed!\r\n Visit our github for more details.";

        public string DeleteSuccessTitle = "Deleted";

        public string DeleteSuccessMessage = "Information about that TV show no longer available.";

        public string UnknownErrorTitle = "Error Occured";
    }
}
