using System;
using System.Collections.Generic;
using System.Text;

namespace VoiceRecognition.Model
{
    public class PeopleDto
    {
        public PeopleDto() { }
        public PeopleDto(People people)
        {
            this.People = people;
        }
        public string AzureId { get; set; }
        public string EnrollmentStatusColor { get { if (EnrollmentStatus == "Enrolled") return "Red"; return "Blue"; } }
        public string EnrollmentStatus { get; set; }
        public string EnrollmentStatusMessage { get { 
                if (EnrollmentStatus == "Enrolled") return "User is Enrolled"; 
                else if(EnrollmentStatus=="Enrolling") return "User is enrolling";
                return "user not enrolled";
            } }
        public string FirstName { get { return People.FirstName; } }
        public string LastName { get { return People.LastName; } }
        public string PhoneNumber { get { return People.PhoneNumber; } }
        public string FullName { get { return People.FirstName + " " + People.LastName; } }
        public People People { get; set; }

    }
}
