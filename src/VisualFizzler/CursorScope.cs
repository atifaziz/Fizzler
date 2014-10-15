#region License, Terms and Author(s)
//
// Schnell - Wiki widgets
// Copyright (c) 2007 Atif Aziz. All rights reserved.
//
//  Author(s):
//      Atif Aziz, http://www.raboof.com
//
// This library is free software; you can redistribute it and/or modify it 
// under the terms of the GNU Lesser General Public License as published by 
// the Free Software Foundation; either version 2.1 of the License, or (at 
// your option) any later version.
//
// This library is distributed in the hope that it will be useful, but WITHOUT
// ANY WARRANTY; without even the implied warranty of MERCHANTABILITY or 
// FITNESS FOR A PARTICULAR PURPOSE. See the GNU Lesser General Public 
// License for more details.
//
// You should have received a copy of the GNU Lesser General Public License
// along with this library; if not, write to the Free Software Foundation, 
// Inc., 59 Temple Place, Suite 330, Boston, MA 02111-1307 USA 
//
#endregion

namespace VisualFizzler
{
    using System.Windows.Forms;

    internal static class CurrentCursorScope
    {
        public static SingletonScope<Cursor, Helper> EnterWait()
        {
            return Enter(Cursors.WaitCursor);
        }

        public static SingletonScope<Cursor, Helper> Enter(Cursor cursor)
        {
            return new SingletonScope<Cursor, Helper>(cursor);
        }

        internal struct Helper : ISingletonScopeHelper<Cursor>
        {
            public Cursor Install(Cursor temp)
            {
                Cursor save = Cursor.Current;
                Cursor.Current = temp;
                return save;
            }

            public void Restore(Cursor old)
            {
                Cursor.Current = old;
            }
        }
    }
}