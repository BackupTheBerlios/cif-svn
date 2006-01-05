using System;
using System.Collections.Generic;
using System.Text;

namespace Tracker
{

    public class ServerHelper
    {

        #region Enums

        public enum StatusCodes
        {
            Success = TRK_SUCCESS,
            VersionMismatch = TRK_E_VERSION_MISMATCH,
            OutOfMemeory = TRK_E_OUT_OF_MEMORY,
            BadHandle = TRK_E_BAD_HANDLE,
            BadInputPointer = TRK_E_BAD_INPUT_POINTER,
            BadInputValue = TRK_E_BAD_INPUT_VALUE,
            DataTruncated = TRK_E_DATA_TRUNCATED,
            NoMoreData = TRK_E_NO_MORE_DATA,
            ListNotInitalized = TRK_E_LIST_NOT_INITIALIZED,
            EndOfList = TRK_E_END_OF_LIST,
            NotLoggedIn = TRK_E_NOT_LOGGED_IN,
            ServerNotPrepared = TRK_E_SERVER_NOT_PREPARED,
            BadDatabaseVersion = TRK_E_BAD_DATABASE_VERSION,
            UnableToConnect = TRK_E_UNABLE_TO_CONNECT,
            UnableToDisconnect = TRK_E_UNABLE_TO_DISCONNECT,
            UnableToStartTimer = TRK_E_UNABLE_TO_START_TIMER,
            NoDataSources = TRK_E_NO_DATA_SOURCES,
            NoProjects = TRK_E_NO_PROJECTS,
            FailedWrite = TRK_E_WRITE_FAILED,
            PermissionDenied = TRK_E_PERMISSION_DENIED,
            SetFieldDenied = TRK_E_SET_FIELD_DENIED,
            ItemNotFound = TRK_E_ITEM_NOT_FOUND,
            CannotAccessDatabase = TRK_E_CANNOT_ACCESS_DATABASE,
            CannotAccessQuery = TRK_E_CANNOT_ACCESS_QUERY,
            CannotAccesIntray = TRK_E_CANNOT_ACCESS_INTRAY,
            CannotOpenFile = TRK_E_CANNOT_OPEN_FILE,
            InvalidDBMSType = TRK_E_INVALID_DBMS_TYPE,
            InvalidRecordType = TRK_E_INVALID_RECORD_TYPE,
            InvalidField = TRK_E_INVALID_FIELD,
            InvalidChoice = TRK_E_INVALID_CHOICE,
            InvalidUser = TRK_E_INVALID_USER,
            InvalidSubmitter = TRK_E_INVALID_SUBMITTER,
            InvalidOwner = TRK_E_INVALID_OWNER,
            InvalidDate = TRK_E_INVALID_DATE,
            InvalidStoredQuery = TRK_E_INVALID_STORED_QUERY,
            InvalidMode = TRK_E_INVALID_MODE,
            InvalidMessage = TRK_E_INVALID_MESSAGE,
            ValueOutOfRange = TRK_E_VALUE_OUT_OF_RANGE,
            WRONGFIELDTYPE = TRK_E_WRONG_FIELD_TYPE,
            NOCURRENTRECORD = TRK_E_NO_CURRENT_RECORD,
            NOCURRENTNOTE = TRK_E_NO_CURRENT_NOTE,
            NOCURRENTATTACHEDFILE = TRK_E_NO_CURRENT_ATTACHED_FILE,
            NOCURRENTASSOCIATION = TRK_E_NO_CURRENT_ASSOCIATION,
            NORECORDBEGIN = TRK_E_NO_RECORD_BEGIN,
            NOMODULE = TRK_E_NO_MODULE,
            USERCANCELLED = TRK_E_USER_CANCELLED,
            SEMAPHORETIMEOUT = TRK_E_SEMAPHORE_TIMEOUT,
            SEMAPHOREERROR = TRK_E_SEMAPHORE_ERROR,
            INVALIDSERVERNAME = TRK_E_INVALID_SERVER_NAME,
            NOTLICENSED = TRK_E_NOT_LICENSED,
            NUMBEROFERRORCODES = TRK_NUMBER_OF_ERROR_CODES,
            ERRORCODEBASE = TRKEXP_ERROR_CODE_BASE,
            EXPORTWRONGVERSION = TRKEXP_E_EXPORT_WRONG_VERSION,
            EXPORTSETNOTINIT = TRKEXP_E_EXPORTSET_NOT_INIT,
            NOEXPSETNAME = TRKEXP_E_NO_EXPSET_NAME,
            BADEXPSETNAME = TRKEXP_E_BAD_EXPSET_NAME,
            EXPSETFAILCREATE = TRKEXP_E_EXPSET_FAIL_CREATE,
            IMPORTMAPNOTINIT = TRKEXP_E_IMPORTMAP_NOT_INIT,
            NOIMPMAPNAME = TRKEXP_E_NO_IMPMAP_NAME,
            BADIMPMAPNAME = TRKEXP_E_BAD_IMPMAP_NAME,
            IMPMAPFAILCREATE = TRKEXP_E_IMPMAP_FAIL_CREATE,
            IMPVALIDATEFAIL = TRKEXP_E_IMP_VALIDATE_FAIL,
            USERNOEXIST = TRKEXP_E_USER_NOEXIST,
            USERADD = TRKEXP_E_USER_ADD,
            IMPORTNOTINIT = TRKEXP_E_IMPORT_NOT_INIT,
            BADEMBEDDEDQUOTEARG = TRKEXP_E_BAD_EMBEDDED_QUOTE_ARG,
            BADDATEFORMATARG = TRKEXP_E_BAD_DATEFORMAT_ARG,
            BADTIMEFORMATARG = TRKEXP_E_BAD_TIMEFORMAT_ARG,
            BADCHOICEOPTIONARG = TRKEXP_E_BAD_CHOICE_OPTION_ARG,
            BADUSEROPTIONARG = TRKEXP_E_BAD_USER_OPTION_ARG,
            BADNUMBEROPTIONARG = TRKEXP_E_BAD_NUMBER_OPTION_ARG,
            BADDATEOPTIONARG = TRKEXP_E_BAD_DATE_OPTION_ARG,
            ALLNOTESSELECTED = TRKEXP_E_ALL_NOTES_SELECTED,
            READEXPORTHDR = TRKEXP_E_READ_EXPORTHDR,
            WRITEEXPORTHDR = TRKEXP_E_WRITE_EXPORTHDR,
            READRECORDHDR = TRKEXP_E_READ_RECORDHDR,
            WRITERECORDHDR = TRKEXP_E_WRITE_RECORDHDR,
            WRITEFIELD = TRKEXP_E_WRITE_FIELD,
            OPENFILE = TRKEXP_E_OPEN_FILE,
            READFIELD = TRKEXP_E_READ_FIELD,
            READFIELDWRONGTYPE = TRKEXP_E_READ_FIELD_WRONG_TYPE,
            BADITEMTYPE = TRKEXP_E_BAD_ITEM_TYPE,
            READFROMDB = TRKEXP_E_READ_FROM_DB,
            WRITETODB = TRKEXP_E_WRITE_TO_DB,
            BADDATE = TRKEXP_E_BAD_DATE,
            BADCHOICE = TRKEXP_E_BAD_CHOICE,
            BADNUMBER = TRKEXP_E_BAD_NUMBER,
            OPENERRORLOG = TRKEXP_E_OPEN_ERRORLOG,
            BADERRORLOGPATH = TRKEXP_E_BAD_ERRORLOG_PATH,
            LOGGINGERROR = TRKEXP_E_LOGGING_ERROR,
            IMPORTPERMISSION = TRKEXP_E_IMPORT_PERMISSION,
            EXPORTPERMISSION = TRKEXP_E_EXPORT_PERMISSION,
            NEWUSERPERMISSION = TRKEXP_E_NEW_USER_PERMISSION,
            CLOSEERRORLOG = TRKEXP_E_CLOSE_ERRORLOG,
            NEWCHOICESYSFLD = TRKEXP_E_NEWCHOICE_SYSFLD,
            EXTRAFIELDS = TRKEXP_E_EXTRA_FIELDS,
            EXPNUMBEROFERRORCODES = TRKEXP_NUMBER_OF_ERROR_CODES
        }

        public enum StorageMode
        {
            BinaryMode,
            ASCIIMode
        }

        #endregion

        #region Const

        public const double DateModifier = 25568.791666666668;
        public const int MaxBufferLength = 0xff;
        public const int TRK_24HOUR = 2;
        public const int TRK_24HOUR_LEADING_ZERO = 3;
        public const int TRK_ADD_USER = 1;
        public const int TRK_BACKSLASH_QUOTE = 2;
        public const int TRK_CANCEL_IMPORT = 3;
        public const int TRK_CANCEL_INTRAY = 1;
        public const int TRK_CANCEL_QUERY = 2;
        public const int TRK_CONTROL_PANEL_DATE = 1;
        public const int TRK_CONTROL_PANEL_TIME = 1;
        public const int TRK_DBASE_FORMAT = 2;
        public const int TRK_DEFAULT_CHOICE = 1;
        public const int TRK_DEFAULT_NUMBER = 1;
        public const int TRK_DOUBLE_QUOTE = 1;
        public const int TRK_E_BAD_DATABASE_VERSION = 12;
        public const int TRK_E_BAD_HANDLE = 3;
        public const int TRK_E_BAD_INPUT_POINTER = 4;
        public const int TRK_E_BAD_INPUT_VALUE = 5;
        public const int TRK_E_CANNOT_ACCESS_DATABASE = 0x16;
        public const int TRK_E_CANNOT_ACCESS_INTRAY = 0x18;
        public const int TRK_E_CANNOT_ACCESS_QUERY = 0x17;
        public const int TRK_E_CANNOT_OPEN_FILE = 0x19;
        public const int TRK_E_DATA_TRUNCATED = 6;
        public const int TRK_E_END_OF_LIST = 9;
        public const int TRK_E_INTERNAL_ERROR = 0x4e20;
        public const int TRK_E_INVALID_CHOICE = 0x1d;
        public const int TRK_E_INVALID_DATE = 0x21;
        public const int TRK_E_INVALID_DBMS_TYPE = 0x1a;
        public const int TRK_E_INVALID_FIELD = 0x1c;
        public const int TRK_E_INVALID_MESSAGE = 0x24;
        public const int TRK_E_INVALID_MODE = 0x23;
        public const int TRK_E_INVALID_OWNER = 0x20;
        public const int TRK_E_INVALID_RECORD_TYPE = 0x1b;
        public const int TRK_E_INVALID_SERVER_NAME = 0x30;
        public const int TRK_E_INVALID_STORED_QUERY = 0x22;
        public const int TRK_E_INVALID_SUBMITTER = 0x1f;
        public const int TRK_E_INVALID_USER = 30;
        public const int TRK_E_ITEM_NOT_FOUND = 0x15;
        public const int TRK_E_LIST_NOT_INITIALIZED = 8;
        public const int TRK_E_NO_CURRENT_ASSOCIATION = 0x2a;
        public const int TRK_E_NO_CURRENT_ATTACHED_FILE = 0x29;
        public const int TRK_E_NO_CURRENT_NOTE = 40;
        public const int TRK_E_NO_CURRENT_RECORD = 0x27;
        public const int TRK_E_NO_DATA_SOURCES = 0x10;
        public const int TRK_E_NO_MODULE = 0x2c;
        public const int TRK_E_NO_MORE_DATA = 7;
        public const int TRK_E_NO_PROJECTS = 0x11;
        public const int TRK_E_NO_RECORD_BEGIN = 0x2b;
        public const int TRK_E_NOT_LICENSED = 0x31;
        public const int TRK_E_NOT_LOGGED_IN = 10;
        public const int TRK_E_OUT_OF_MEMORY = 2;
        public const int TRK_E_PERMISSION_DENIED = 0x13;
        public const int TRK_E_SEMAPHORE_ERROR = 0x2f;
        public const int TRK_E_SEMAPHORE_TIMEOUT = 0x2e;
        public const int TRK_E_SERVER_NOT_PREPARED = 11;
        public const int TRK_E_SET_FIELD_DENIED = 20;
        public const int TRK_E_UNABLE_TO_CONNECT = 13;
        public const int TRK_E_UNABLE_TO_DISCONNECT = 14;
        public const int TRK_E_UNABLE_TO_START_TIMER = 15;
        public const int TRK_E_USER_CANCELLED = 0x2d;
        public const int TRK_E_VALUE_OUT_OF_RANGE = 0x25;
        public const int TRK_E_VERSION_MISMATCH = 1;
        public const int TRK_E_WRITE_FAILED = 0x12;
        public const int TRK_E_WRONG_FIELD_TYPE = 0x26;
        public const int TRK_FAIL_CHOICE = 0;
        public const int TRK_FAIL_DATE = 0;
        public const int TRK_FAIL_NUMBER = 0;
        public const int TRK_FAIL_USER = 0;
        public const int TRK_FIELD_TYPE_CHOICE = 1;
        public const int TRK_FIELD_TYPE_DATE = 4;
        public const int TRK_FIELD_TYPE_ELAPSED_TIME = 8;
        public const int TRK_FIELD_TYPE_NONE = 0;
        public const int TRK_FIELD_TYPE_NUMBER = 3;
        public const int TRK_FIELD_TYPE_OWNER = 6;
        public const int TRK_FIELD_TYPE_STRING = 2;
        public const int TRK_FIELD_TYPE_SUBMITTER = 5;
        public const int TRK_FIELD_TYPE_USER = 7;
        public const int TRK_FILE_ASCII = 1;
        public const int TRK_FILE_BINARY = 0;
        public const int TRK_INTERNAL_ERROR_CODE_BASE = 0x4e20;
        public const int TRK_LAST_CALLBACK_MSG = 11;
        public const int TRK_LIST_ADD_HEAD = 0;
        public const int TRK_LIST_ADD_TAIL = 1;
        public const int TRK_MAX_STRING = 0xff;
        public const int TRK_MSG_API_EXIT = 2;
        public const int TRK_MSG_API_TRACE = 1;
        public const int TRK_MSG_DATA_TRUNCATED = 5;
        public const int TRK_MSG_FORCE_LOGOUT = 6;
        public const int TRK_MSG_HANDLED = 1;
        public const int TRK_MSG_IMPORT_ERROR = 7;
        public const int TRK_MSG_IMPORT_PROGRESS = 10;
        public const int TRK_MSG_INTRAY_PROGRESS = 8;
        public const int TRK_MSG_INVALID_FIELD_VALUE = 4;
        public const int TRK_MSG_NOT_HANDLED = 0;
        public const int TRK_MSG_ODBC_ERROR = 3;
        public const int TRK_MSG_QUERY_PROGRESS = 9;
        public const int TRK_NEW_CHOICE = 2;
        public const int TRK_NO_KEEP_ALIVE = 4;
        public const int TRK_NO_TIMER = 5;
        public const int TRK_NUMBER_OF_ERROR_CODES = 0x31;
        public const int TRK_READ_ONLY = 0;
        public const int TRK_READ_WRITE = 2;
        public const long TRK_RECORD_TYPE = 1;
        public const int TRK_SET_CURRENT = 1;
        public const int TRK_SET_TO_SPECIFIED = 2;
        public const int TRK_SUCCESS = 0;
        public const int TRK_TRKTOOL_ATTRIBUTE_ID_BASE = 0;
        public const int TRK_USE_DEFAULT_DBMS_LOGIN = 2;
        public const int TRK_USE_INI_FILE_DBMS_LOGIN = 0;
        public const int TRK_USE_SPECIFIED_DBMS_LOGIN = 1;
        public const int TRK_USER_ATTRIBUTE_ID_BASE = 0x3e8;
        public const int TRK_VERSION_ID = 0x7a121;
        public const int TRKEXP_E_ALL_NOTES_SELECTED = 0x2724;
        public const int TRKEXP_E_BAD_CHOICE = 0x2731;
        public const int TRKEXP_E_BAD_CHOICE_OPTION_ARG = 0x2720;
        public const int TRKEXP_E_BAD_DATE = 0x2730;
        public const int TRKEXP_E_BAD_DATE_OPTION_ARG = 0x2723;
        public const int TRKEXP_E_BAD_DATEFORMAT_ARG = 0x271e;
        public const int TRKEXP_E_BAD_EMBEDDED_QUOTE_ARG = 0x271d;
        public const int TRKEXP_E_BAD_ERRORLOG_PATH = 0x2734;
        public const int TRKEXP_E_BAD_EXPSET_NAME = 0x2713;
        public const int TRKEXP_E_BAD_IMPMAP_NAME = 0x2717;
        public const int TRKEXP_E_BAD_ITEM_TYPE = 0x272d;
        public const int TRKEXP_E_BAD_NUMBER = 0x2732;
        public const int TRKEXP_E_BAD_NUMBER_OPTION_ARG = 0x2722;
        public const int TRKEXP_E_BAD_TIMEFORMAT_ARG = 0x271f;
        public const int TRKEXP_E_BAD_USER_OPTION_ARG = 0x2721;
        public const int TRKEXP_E_CLOSE_ERRORLOG = 0x2739;
        public const int TRKEXP_E_EXPORT_PERMISSION = 0x2737;
        public const int TRKEXP_E_EXPORT_WRONG_VERSION = 0x2710;
        public const int TRKEXP_E_EXPORTSET_NOT_INIT = 0x2711;
        public const int TRKEXP_E_EXPSET_FAIL_CREATE = 0x2714;
        public const int TRKEXP_E_EXTRA_FIELDS = 0x273b;
        public const int TRKEXP_E_IMP_VALIDATE_FAIL = 0x2719;
        public const int TRKEXP_E_IMPMAP_FAIL_CREATE = 0x2718;
        public const int TRKEXP_E_IMPORT_NOT_INIT = 0x271c;
        public const int TRKEXP_E_IMPORT_PERMISSION = 0x2736;
        public const int TRKEXP_E_IMPORTMAP_NOT_INIT = 0x2715;
        public const int TRKEXP_E_LOGGING_ERROR = 0x2735;
        public const int TRKEXP_E_NEW_USER_PERMISSION = 0x2738;
        public const int TRKEXP_E_NEWCHOICE_SYSFLD = 0x273a;
        public const int TRKEXP_E_NO_EXPSET_NAME = 0x2712;
        public const int TRKEXP_E_NO_IMPMAP_NAME = 0x2716;
        public const int TRKEXP_E_OPEN_ERRORLOG = 0x2733;
        public const int TRKEXP_E_OPEN_FILE = 0x272a;
        public const int TRKEXP_E_READ_EXPORTHDR = 0x2725;
        public const int TRKEXP_E_READ_FIELD = 0x272b;
        public const int TRKEXP_E_READ_FIELD_WRONG_TYPE = 0x272c;
        public const int TRKEXP_E_READ_FROM_DB = 0x272e;
        public const int TRKEXP_E_READ_RECORDHDR = 0x2727;
        public const int TRKEXP_E_USER_ADD = 0x271b;
        public const int TRKEXP_E_USER_NOEXIST = 0x271a;
        public const int TRKEXP_E_WRITE_EXPORTHDR = 0x2726;
        public const int TRKEXP_E_WRITE_FIELD = 0x2729;
        public const int TRKEXP_E_WRITE_RECORDHDR = 0x2728;
        public const int TRKEXP_E_WRITE_TO_DB = 0x272f;
        public const int TRKEXP_ERROR_CODE_BASE = 0x2710;
        public const int TRKEXP_NUMBER_OF_ERROR_CODES = 0x273b;

        #endregion

        public ServerHelper()
        {

        }

        public void CheckStatus(string exceptionMessage, int status)
        {
            if ((StatusCodes)status == StatusCodes.Success)
                return;

            throw new InvalidOperationException(string.Format("{0}: Code = {1}", exceptionMessage, Enum.GetName(typeof(StatusCodes), status)));
        }

        public string CleanupString(string dirtyString)
        {
            dirtyString = dirtyString.Replace("\0", "");
            return dirtyString.Trim();
        }

        public string ConvertDateToString(int dateTime)
        {
            return System.DateTime.FromOADate((((((double)dateTime) / 60) / 60) / 24) + DateModifier).ToString();
        }

        public string MakeBigEmptyString(int size)
        {
            return new string('\0', size);
        }

    }

}
