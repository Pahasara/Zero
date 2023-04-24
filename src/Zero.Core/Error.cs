// Copyright (c) 2023 Dewnith Fernando @github.com/Pahasara.
// Licensed under the MIT license.

namespace Zero.Core
{
    public class Error
    {
        // Below strings will be shown under Message Box
        // <--Message_UI.msgText
        public string Update = "An error occured during update!";

        public string Rating = "An error occured during rating update!";

        public string Insert = "is available! Try another index.";

        public string Delete = "An error occured during delete!";

        public string Search = "An error occured during search!";


        // Below strings will be shown under Status label
        // <--Main_UI.labelST
        public string SearchNotFound = "No matching";

        public string DatabaseEmpty = "Database empty";

        public string Forward = "Update failed";

        public string IndexNull = "Empty index";

        public string WatchedNull = "Empty watched";

        public string EpisodesNull = "Empty episodes";
    }
}
