using System;
using System.Collections.Generic;
class Data
{
    public string Username {get; set;}
    public string Password {get; set;}
    public List<string> Location {get; set;}
    public string Content {get; set;}
    public string DateRange {get; set;}
    public List<string> LeadStatus {get; set;}
    public List<string> Programs {get; set;}

    public Data(string Username, string Password, List<string> Location, string Content, string DateRange, List<string> LeadStatus, List<string> Programs)
    {
        this.Username = Username;
        this.Password = Password;
        this.Location = Location;
        this.Content = Content;
        this.DateRange = DateRange;
        this.LeadStatus = LeadStatus;
        this.Programs = Programs;
    }
}