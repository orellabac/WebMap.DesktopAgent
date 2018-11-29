using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.ComponentModel.Composition.Hosting;
using System.Diagnostics;
using System.Windows.Forms;

namespace Mobilize
{

    /// <summary>
    /// Desktop Agent is just an empty shell.
    /// Actual functionality is provided thru plugins which will handle the actual code
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class PluginsLoader<T>
    {
        /// <summary>
        /// A MEF container to load the plugins
        /// </summary>
        private CompositionContainer _Container;

		[ImportMany]
		public IEnumerable<T> Plugins
		{
			get;
			set;
		}

        public PluginsLoader(string path)
		{
            try
            {
                var fullPath = System.IO.Path.GetFullPath(path);
                Trace.TraceInformation("Plugins Directory Path:"+ fullPath);

			DirectoryCatalog directoryCatalog = new DirectoryCatalog(path);

			//An aggregate catalog that combines multiple catalogs
			var catalog = new AggregateCatalog(directoryCatalog);

			// Create the CompositionContainer with all parts in the catalog (links Exports and Imports)
			_Container = new CompositionContainer(catalog);

			//Fill the imports of this object
			_Container.ComposeParts(this);
		}
            catch (System.Exception ex)
            {
                ErrorLog Err = new ErrorLog();
                string error = "Error Load Plugins: " + ex.Message + System.Environment.NewLine
                    + "Please take a look if this path exist: " + path;
                Err.generateErrorLog(error, ex.StackTrace);
                MessageBox.Show(error);
                throw;
            }

		}
    }
}
