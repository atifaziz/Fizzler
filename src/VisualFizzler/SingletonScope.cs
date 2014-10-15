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
    #region Imports

    using System;
    using System.Diagnostics;

    #endregion

    //
    // NOTE: To use SingletonScope and ISingletonScopeHelper with value 
    // types, use Nullable<T>. For example, if the type of value to scope
    // is ThreadPriority then use ISingletonScopeHelper<ThreadPriority?>
    // and SingletonScope<ThreadPriority?>.
    //

    //
    // In debug builds, this type is defined as a class so a finalizer
    // can be used to detect an undisposed scope.
    //
    
    /// <summary>
    /// Designed to change a singleton and scope that change. After exiting
    /// the scope, the singleton is restored to its value prior to entering
    /// the scope.
    /// </summary>

    #if !DEBUG
    internal struct SingletonScope<T, H> 
    #else
    internal sealed class SingletonScope<T, H> 
    #endif
        : IDisposable 
        where H : ISingletonScopeHelper<T>, new()
    {
        private T _old;
        private bool _disposed;
        
        public SingletonScope(T temp)
        {
            _disposed = false;
            _old = Helper.Install(temp);
        }

        private static H Helper
        {
            get { return new H(); }
        }

        public void Dispose()
        {
            //
            // First, transfer fields to stack then nuke the fields.
            //
            
            var old = _old;
            
            //
            // Already disposed?
            //

            if (_disposed)
                return;
            
            _old = default(T);
            _disposed = true;

            //
            // Shazam! Restore the old value.
            //
            
            Helper.Restore(old);

            #if DEBUG
            GC.SuppressFinalize(this); // Only when defined as a class!
            #endif
        }

        #if DEBUG
        
        //
        // This finalizer is used to detect an undisposed scope. This will
        // only indicate that the scope was not disposed but (unfortunately)
        // not which one and where since GC will probably collect much later
        // than it should have been disposed.
        //
        // NOTE! Finalizer may run even when requested not to.
        // It has been observed that finalizer may run even  when requested 
        // not to via GC.SuppressFinalize. As a result, an explicit check
        // has been added to remember if the object has been disposed or not.
        //
        
        ~SingletonScope()
        {
            if (!_disposed) 
                Debug.Fail("Scope for " + typeof(T).FullName + " not disposed!");
        }
        
        #endif
    }
}
