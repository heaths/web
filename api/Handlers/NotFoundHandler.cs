﻿namespace WixToolset.Web.Api.Handlers
{
    using System;
    using NLog;
    using TinyWebStack;
    using TinyWebStack.Models;

    [Route("404")]
    [Route("notfound")]
    public class NotFoundHandler : IGet, IOutput<FileTransmission>
    {
        private static Logger Log = LogManager.GetLogger("notfound");

        public string _RawQueryString { private get; set; }

        public FileTransmission Output { get; private set; }

        public IRequest Request { private get; set; }

        public Status Get()
        {
            // Usually the error information is passed via the query string so
            // try to process that raw value.
            //
            string[] errorAndUrl = null;
            if (!String.IsNullOrEmpty(_RawQueryString))
            {
                errorAndUrl = this._RawQueryString.Split(new[] { ';' }, 2);
            }

            // If the query string was not provided nor formatted properly try to use the
            // referrer.
            //
            if (errorAndUrl == null || errorAndUrl.Length != 2)
            {
                errorAndUrl = new[] { "404", this.Request.Referrer == null ? "Unknown" : this.Request.Referrer.AbsoluteUri };
            }

            Log.Info("url: {0}  referrer: {1}", this.Request.Url.AbsoluteUri, errorAndUrl[1]);

            this.Output = new FileTransmission("text/html", "/notfound/index.html");

            return Status.NotFound;
        }
    }
}
