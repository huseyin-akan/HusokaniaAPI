
using MiniETrade.Application.Common.Abstractions.Logging;
using MiniETrade.Application.Features.Products.Commands;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.UnitTests.Common.Behaviours
{
    public class LoggingBehaviourTests
    {
        record LoggableCommandTestObjectForSuccess : IRequest<ILoggableRequestResponse>, ILoggableRequest;
        class LoggableCommandTestObjectForSuccessHandler : IRequestHandler<LoggableCommandTestObjectForSuccess, ILoggableRequestResponse>
        {
            public Task<ILoggableRequestResponse> Handle(LoggableCommandTestObjectForSuccess request, CancellationToken cancellationToken) { return null; }
        }
        record LoggableCommandTestObjectForError : IRequest<ILoggableRequestResponse>, ILoggableRequest;
        class LoggableCommandTestObjectForErrorHandler : IRequestHandler<LoggableCommandTestObjectForSuccess, ILoggableRequestResponse>
        {
            public Task<ILoggableRequestResponse> Handle(LoggableCommandTestObjectForSuccess request, CancellationToken cancellationToken) { return null; }
        }
        record ILoggableRequestResponse;

        private readonly IMediator _mediator;

        public LoggingBehaviourTests(IMediator mediator)
        {
            _mediator = mediator;
        }

        [Fact]
        public void LoggingBehaviourShouldLogOnlyObjectsImplementingILoggableRequest()
        {
            //TODO-HUS
        }

        [Fact]
        public void LogResponseMethodShouldBeExecutedOnce()
        {
            //TODO-HUS
        }
    }
}
