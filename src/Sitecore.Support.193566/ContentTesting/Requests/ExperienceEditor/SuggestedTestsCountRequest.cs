// (C) 2014 Sitecore Corporation A/S. All rights reserved.

using System;
using System.Linq;
using Sitecore.ContentTesting;
using Sitecore.ContentTesting.Configuration;
using Sitecore.ContentTesting.Data;
using Sitecore.Diagnostics;
using Sitecore.ExperienceEditor.Speak.Server.Responses;
using Sitecore.ContentTesting.Requests.ExperienceEditor;

namespace Sitecore.Support.ContentTesting.Requests.ExperienceEditor
{
    /// <summary>
    /// Experience Editor request handler to report the number of suggested tests.
    /// </summary>
    public class SuggestedTestsCountRequest : PipelineProcessorRequestBase
    {
        // todo: 8.2: Change base class to PipelineProcessorRequest<ValueItemContext>

        /// <summary>The <see cref="IContentTestStore"/> to read test data from.</summary>
        private readonly IContentTestStore _contentTestStore = null;

        /// <summary>
        /// Initializes a new instance of the <see cref="SuggestedTestsCountRequest"/> class.
        /// </summary>
        public SuggestedTestsCountRequest()
            : this(ContentTestingFactory.Instance.ContentTestStore)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="SuggestedTestsCountRequest"/> class.
        /// </summary>
        /// <param name="contentTestStore">The <see cref="IContentTestStore"/> to read tests from.</param>
        public SuggestedTestsCountRequest([NotNull]IContentTestStore contentTestStore)
        {
            Assert.ArgumentNotNull(contentTestStore, "contentTestStore");

            _contentTestStore = contentTestStore;
        }

        /// <summary>
        /// Handler the request.
        /// </summary>
        /// <returns>The number of suggested tests.</returns>
        public override PipelineProcessorResponseValue ProcessRequest()
        {
            if (!Settings.IsAutomaticContentTestingEnabled)
            {
               return new PipelineProcessorResponseValue
               {
                  Value = false
               };
            }
            var tests = _contentTestStore.GetSuggestedTests(null);
            var count = tests.Count();

            return new PipelineProcessorResponseValue
            {
                Value = count > Settings.SuggestedTestsMaximum ? Settings.SuggestedTestsMaximum + "+" : count.ToString()
            };
        }

        [Obsolete("Use ProcessRequest() instead")]
        public override PipelineProcessorResponseValue Process()
        {
            return ProcessRequest();
        }
    }
}
