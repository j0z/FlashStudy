using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FlashStudy
{
    class Stack
    {
        public List<Card> cards;
        public string stackName = "default stack";

        void Stack(string stackName)
        {
            cards = new List<Card>();
            this.stackName = stackName;
        }


    }
}
