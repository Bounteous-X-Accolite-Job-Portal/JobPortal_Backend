﻿using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Bountous_X_Accolite_Job_Portal.Models
{
    public class JobApplication
    {
        [Key]
        public Guid ApplicationId { get; set; }
        public Guid? CandidateId { get; set; }
        public virtual Candidate? Candidate { get; set; }   

        public Guid? JobId { get; set; }
        public virtual Job? Job { get; set; }    

        public DateTime AppliedOn { get; set; }

        public int? StatusId { get; set; }
        public virtual Status? Status { get; set; }



    }
}
