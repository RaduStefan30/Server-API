using StudentApp.API.Helpers;
using StudentApp.API.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace StudentApp.API.Data
{
    public interface IStudentRepository
    {
        void Add<T>(T entity) where T : class;
        void Delete<T>(T entity) where T : class;
        Task<bool> SaveAll();
        Task<PagedList<User>> GetUsers(UserParams userParams);
        Task<User> GetUser(int id);
        Task<Photo> GetPhoto(int id);
        Task<Photo> GetSetPhoto(int userId);
        // Task<Note> GetNote(int id);
        Task<PagedList<Attendance>> GetAttendance(AttendanceParams attendanceParams);
        Task<Message> GetMessage(int id); 
        Task<PagedList<Message>> GetMessages(MessageParams messageParams);
        Task<IEnumerable<Message>> GetConversation(int userId, int receiverId);
        Task<PagedList<Course>> GetCourses(CoursesParams coursesParams); 
        Task<Course> GetCourse(int id); 

    }
}
