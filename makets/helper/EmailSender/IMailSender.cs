using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace makets.helper.EmailSender
{
    public interface IMailSender
    {
        public bool SendMail();
    }
}
