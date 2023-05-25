using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FORTRAN_COMMENTS.ViewInterfaces
{
    internal interface IActionsOnCode
    {
        void CutAction();
        void CopyAction();
        void DeleteAction();
        void InsertAction();
        void SelectAllAction();
    }

    internal interface IActionsOnStack
    {
        void UndoActions();
        void RedoActions();
    }
}
