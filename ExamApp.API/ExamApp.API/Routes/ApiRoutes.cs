namespace ExamApp.API.Routes;

public struct ApiRoutes
    {
        private const string Root = "api";
        private const string Version = "v1";
        private const string Base = Root + "/" + Version;


        
        public struct Exam
        {
            public const string GetAll = Base + "/exam";
            public const string Get = Base + "/exam/{examId}";
            public const string Update = Base + "/exam/{examId}";
            public const string UpdatePupilExam = Base + "/exam/pupil/{examId}";
            public const string Delete = Base + "/exam/{examId}";
            public const string Create = Base + "/exam";
        }
        
        public struct Grade
        {
            public const string GetAll = Base + "/grade";
            public const string Get = Base + "/grade/{gradeId}";
            public const string Update = Base + "/grade/{gradeId}";
            public const string Delete = Base + "/grade/{gradeId}";
            public const string Create = Base + "/grade/{grade}";
        }
        public struct Lesson
        {
            public const string GetAll = Base + "/lesson";
            public const string Get = Base + "/lesson/{lessonId}";
            public const string Update = Base + "/lesson/{lessonId}";
            public const string Delete = Base + "/lesson/{lessonId}";
            public const string Create = Base + "/lesson";
        }
        public struct Pupil
        {
            public const string GetAll = Base + "/pupil";
            public const string Get = Base + "/pupil/{pupilId}";
            public const string Update = Base + "/pupil/{pupilId}";
            public const string Delete = Base + "/pupil/{pupilId}";
            public const string Create = Base + "/pupil";
        }
       
        public struct Teacher
        {
            public const string GetAll = Base + "/teacher";
            public const string Get = Base + "/teacher/{teacherId}";
            public const string Update = Base + "/teacher/{teacherId}";
            public const string Delete = Base + "/teacher/{teacherId}";
            public const string Create = Base + "/teacher";
            public const string CreateLessonGrade = Base + "/teacherGrade/{teacherId}";
        }
     
    }