using LibraryPRN.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibraryPRN.Utility
{
    internal class Common
    {
        private readonly LibraryManagementSystemContext _context;
        private Member? _member;
        public Common(LibraryManagementSystemContext context, Member? member) 
        {
            _context = context;
            _member = member;
        }


    }
}
