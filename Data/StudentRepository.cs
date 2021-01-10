using Microsoft.EntityFrameworkCore;
using StudentApp.API.Helpers;
using StudentApp.API.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace StudentApp.API.Data
{
    public class StudentRepository : IStudentRepository
    {
        private readonly DataContext _context;

        public StudentRepository(DataContext context)
        {
            _context = context;
        }
        public void Add<T>(T entity) where T : class
        {
            _context.Add(entity);
        }

        public void Delete<T>(T entity) where T : class
        {
            _context.Remove(entity);
        }

        // public async Task<Note> GetNote(int id)
        // {
        //     return await _context.AttendanceList.FirstOrDefaultAsync(n => n.Id == id);
        // }

        public async Task<PagedList<Attendance>> GetAttendance(AttendanceParams attendanceParams)
        {
            var attendance = _context.AttendanceList
                .Include(u => u.User)
                .AsQueryable();

            attendance = attendance.Where(u => u.UserId == attendanceParams.UserId).OrderByDescending(n => n.Date);
            return await PagedList<Attendance>.CreateAsync(attendance, attendanceParams.PageNumber, attendanceParams.PageSize);
        }

        public async Task<IEnumerable<Message>> GetConversation(int userId, int receiverId)
        {
            var messages = await _context.Messages
                .Include(u => u.Sender).ThenInclude(p => p.Photos)
                .Include(u => u.Receiver).ThenInclude(p => p.Photos)
                .Where(m => m.ReceiverId == userId && m.ReceiverDeleted == false 
                    && m.SenderId == receiverId 
                    || m.ReceiverId == receiverId && m.SenderId == userId 
                    && m.SenderDeleted == false)
                .OrderByDescending(m => m.MessageSent)
                .ToListAsync();

                return messages;
        }

        public async Task<PagedList<Course>> GetCourses(CoursesParams coursesParams)
        {
            var courses = _context.Courses
                .AsQueryable();

            courses = courses.OrderByDescending(n => n.CourseName);
            return await PagedList<Course>.CreateAsync(courses, coursesParams.PageNumber, coursesParams.PageSize);;
        }

        public async Task<Course> GetCourse(int id)
        {
            return await _context.Courses
                .FirstOrDefaultAsync();
        }

        public async Task<Message> GetMessage(int id)
        {
            return await _context.Messages
                .FirstOrDefaultAsync(m => m.Id == id);
        }

        public async Task<PagedList<Message>> GetMessages(MessageParams messageParams)
        {
            var messages = _context.Messages
                .Include(u => u.Sender).ThenInclude(p => p.Photos)
                .Include(u => u.Receiver).ThenInclude(p => p.Photos)
                .AsQueryable();

            switch (messageParams.MessageContainer)
            {
                case "Inbox":
                    messages = messages.Where(u => u.ReceiverId == messageParams.UserId 
                        && u.ReceiverDeleted == false);
                    break;
                case "Outbox":
                    messages = messages.Where(u => u.SenderId == messageParams.UserId 
                        && u.SenderDeleted == false);
                    break;
                default:
                    messages = messages.Where(u => u.ReceiverId == messageParams.UserId 
                        && u.ReceiverDeleted == false && u.HasRead == false);
                    break;
            }
            messages = messages.OrderByDescending(d => d.MessageSent);

            return await PagedList<Message>.CreateAsync(messages, messageParams.PageNumber, messageParams.PageSize);
        }

        public async Task<Photo> GetPhoto(int id)
        {
            var photo = await _context.Photos.FirstOrDefaultAsync(p => p.Id == id);

            return photo;
        }

        public async Task<Photo> GetSetPhoto(int userId)
        {
            return await _context.Photos.Where(u => u.UserId == userId).FirstOrDefaultAsync(p => p.IsMain);
        }

        public async Task<User> GetUser(int id)
        {
            var user = await _context.Users.Include(p => p.Photos).FirstOrDefaultAsync(u => u.Id == id);

            return user;
        }

        public async Task<PagedList<User>> GetUsers(UserParams userParams)
        {
            var users = _context.Users.Include(p => p.Photos).OrderBy(u => u.LastName).AsQueryable();

            users = users.Where(u => u.Id != userParams.UserId);
            //if (userParams.PageNumber==50)
            //{
            //    users = users.Where(u => u.Faculty == userParams.Faculty && u.Specialization == userParams.Specialization && u.Year == userParams.Year && u.Group == userParams.Group);
            //}
            return await PagedList<User>.CreateAsync(users, userParams.PageNumber, userParams.PageSize);
        }

        public async Task<bool> SaveAll()
        {
            return await _context.SaveChangesAsync() > 0;
        }
    }
}
