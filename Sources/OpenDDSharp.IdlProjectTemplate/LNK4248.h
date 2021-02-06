#ifndef __LNK4248_H__
#define __LNK4248_H__

// incomplete types generate warnings similar to:
// "warning LNK4248: unresolved typeref token (0100001E) for 'CORBA.InterfaceDef'; image may not run"
// providing fake definitions for these types supresses the warnings

#define SUPPRESS_LNK4248_CORBA \
namespace CORBA { \
	class InterfaceDef {}; \
	class Context {}; \
	class NVList {}; \
	class NamedValue {}; \
	class Request {}; \
	class ExceptionList {}; \
	class ContextList {}; \
}

#define SUPPRESS_LNK4248_TAO \
namespace TAO { \
	class ObjectKey {}; \
}


#endif