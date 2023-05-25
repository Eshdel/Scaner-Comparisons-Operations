using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FORTRAN_COMMENTS.Utils
{
    public struct TextInMomentTime
    {
        public string Text { get; private set; }
        public int CursorPosition { get; private set; }
        public TextInMomentTime(string text, int cursorPosition)
        {
            Text = text;
            CursorPosition = cursorPosition;
        }
        public static bool operator ==(TextInMomentTime firstText, TextInMomentTime secondText)
        {
            if (firstText.Text == secondText.Text)
            {
                if (secondText.CursorPosition == firstText.CursorPosition)
                    return true;
            }
            return false;
        }
        public static bool operator !=(TextInMomentTime firstText, TextInMomentTime secondText)
        {
            return !(firstText == secondText);
        }

        public override bool Equals(object obj)
        {
            return obj is TextInMomentTime time &&
                   Text == time.Text &&
                   CursorPosition == time.CursorPosition;
        }

        public override int GetHashCode()
        {
            int hashCode = 75633146;
            hashCode = hashCode * -1521134295 + EqualityComparer<string>.Default.GetHashCode(Text);
            hashCode = hashCode * -1521134295 + CursorPosition.GetHashCode();
            return hashCode;
        }
    }
}
