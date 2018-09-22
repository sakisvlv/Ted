using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ted.Model.Auth;
using Ted.Model.Conversations;
using Ted.Model.Network;
using Ted.Model.PersonalSkills;
using Ted.Model.Posts;

namespace Ted.Dal
{
    public static class Initializer
    {
        public async static Task Initialize(Context context, UserManager<User> userManager, RoleManager<Role> roleManager, string path)
        {
            try
            {

                context.Database.EnsureCreated();
                Directory.CreateDirectory(path);

                List<Tuple<string, string>> names = new List<Tuple<string, string>>() {
               new Tuple<string, string>("Clark","Kent"),
               new Tuple<string, string>("Wade","Wilson"),
               new Tuple<string, string>("Steve","Rogers"),
               new Tuple<string, string>("Bruce","Banner"),
               new Tuple<string, string>("Tony","Stark"),
               new Tuple<string, string>("Peter","Parker"),
               new Tuple<string, string>("Bruce","Wayne"),
               new Tuple<string, string>("Charles", "Xavier"),
               new Tuple<string, string>("Jean", "Grey"),
               new Tuple<string, string>("Scott", "Summers")
            };

                List<string> jobDescriptions = new List<string>() {
               "Killing monsters",
               "Making Coffee",
               "Creating Weapons",
               "Martial arts",
               "Saving the planet"
            };
                List<string> titles = new List<string>() {
               "Assistant",
               "Intern",
               "Chief",
               "Lead Scientist",
               "President"
            };



                // Look for any users or wheel settings
                if (context.Users.Any())
                {
                    return;   // DB has been seeded
                }

                await roleManager.CreateAsync(new Role() { Name = "User" });
                await roleManager.CreateAsync(new Role() { Name = "Admin" });

                User u;
                for (int i = 0; i < 10; i++)
                {
                    u = new User();
                    u.Email = "user" + i + "@gmail.com";
                    u.UserName = u.Email;
                    u.FirstName = names[i].Item1;
                    u.LastName = names[i].Item2;
                    u.PhoneNumber = "6981234567";
                    u.CurrentState = titles[i % 5] + " at X-men";
                    var result = await userManager.CreateAsync(u, "1");
                }
                var users = context.Users.Where(x => true)
                    .Include(x => x.PersonalSkills)
                    .Include(x => x.Educations)
                    .Include(x => x.Experiences)
                    .ToList();
                for (int i = 0; i < users.Count; i++)
                {
                    if (i == 0)
                    {
                        await userManager.AddToRoleAsync(users[i], "Admin");
                    }
                    else
                    {
                        await userManager.AddToRoleAsync(users[i], "User");

                        var education = new Education();
                        education.Grade = 10;
                        education.Degree = "Bachelor";
                        education.Description = "Xavier's School for Gifted Youngsters";
                        education.EndDate = DateTime.Now;
                        education.StartDate = DateTime.Now.AddYears(-4);
                        education.StillThere = false;
                        education.Title = "X-Mansion";
                        education.IsPublic = true;
                        education.Field = "Saving the world";
                        education.Link = "https://en.wikipedia.org/wiki/X-Mansion";
                        users[i].Educations.Add(education);

                        var experiance = new Experience();
                        experiance.IsPublic = true;
                        experiance.Link = "https://en.wikipedia.org/wiki/X-Mansion";
                        experiance.StartDate = DateTime.Now.AddYears(-4);
                        experiance.EndDate = DateTime.Now;
                        experiance.StillThere = true;
                        experiance.Title = titles[i % 5];
                        experiance.Company = "X-men";
                        experiance.Description = jobDescriptions[i % 5];
                        users[i].Experiences.Add(experiance);

                        var personalSkill = new PersonalSkill();
                        personalSkill.IsPublic = true;
                        personalSkill.Title = "Laser Eyes";
                        personalSkill.Description = "Shooting lasers out of the eyes";
                        users[i].PersonalSkills.Add(personalSkill);

                        for (int j = 0; j < 5; j++)
                        {
                            var post = new Post();
                            post.PostedDate = DateTime.Now;
                            post.UserPosts = new List<UserPost>();
                            post.Comments = new List<Comment>();
                            post.Owner = users[i];



                            if (i > 5)
                            {


                                var comment1 = new Comment();
                                var comment2 = new Comment();
                                var comment3 = new Comment();
                                comment1.User = users[1];
                                comment1.Text = "It's truly is!";
                                comment2.User = users[2];
                                comment2.Text = "It's truly is!";
                                comment3.User = users[3];
                                comment3.Text = "It's truly is!";
                                post.UserPosts.Add(new UserPost(users[1], post));
                                post.UserPosts.Add(new UserPost(users[2], post));
                                post.UserPosts.Add(new UserPost(users[3], post));
                                post.Comments.Add(comment1);
                                post.Comments.Add(comment2);
                                post.Comments.Add(comment3);
                            }
                            else
                            {
                                var comment1 = new Comment();
                                comment1.CommentDate = DateTime.Now;
                                var comment2 = new Comment();
                                comment2.CommentDate = DateTime.Now;
                                var comment3 = new Comment();
                                comment3.CommentDate = DateTime.Now;
                                comment1.User = users[6];
                                comment1.Text = "It's truly is!";
                                comment2.User = users[7];
                                comment2.Text = "It's truly is!";
                                comment3.User = users[8];
                                comment3.Text = "It's truly is!";
                                post.UserPosts.Add(new UserPost(users[6], post));
                                post.UserPosts.Add(new UserPost(users[7], post));
                                post.UserPosts.Add(new UserPost(users[8], post));
                                post.Comments.Add(comment1);
                                post.Comments.Add(comment2);
                                post.Comments.Add(comment3);
                            }
                            post.Title = "A nice day today";
                            post.Type = Model.PostType.Article;
                            context.Posts.Add(post);
                        }

                        if (i > 5)
                        {
                            var conversation1 = new Conversation();
                            conversation1.FromUser = users[i];
                            conversation1.ToUser = users[1];
                            conversation1.Messages = new List<Message>();
                            for (int k = 0; k < 15; k++)
                            {
                                var message = new Message();
                                message.DateSended = DateTime.Now;
                                if (k % 2 == 0)
                                {
                                    message.Sender = conversation1.FromUser;
                                    message.Text = "Hello friend for the" + k + "time";
                                }
                                else
                                {
                                    message.Sender = conversation1.ToUser;
                                    message.Text = "Hi there friend for the" + k + "time";
                                }
                                conversation1.Messages.Add(message);
                            }


                            var conversation2 = new Conversation();
                            conversation2.FromUser = users[i];
                            conversation2.ToUser = users[2];
                            conversation2.Messages = new List<Message>();
                            for (int k = 0; k < 15; k++)
                            {
                                var message = new Message();
                                message.DateSended = DateTime.Now;
                                if (k % 2 == 0)
                                {
                                    message.Sender = conversation2.FromUser;
                                    message.Text = "Hello friend for the" + k + "time";
                                }
                                else
                                {
                                    message.Sender = conversation2.ToUser;
                                    message.Text = "Hi there friend for the" + k + "time";
                                }
                                conversation2.Messages.Add(message);
                            }

                            var conversation3 = new Conversation();
                            conversation3.FromUser = users[i];
                            conversation3.ToUser = users[3];
                            conversation3.Messages = new List<Message>();
                            for (int k = 0; k < 15; k++)
                            {
                                var message = new Message();
                                message.DateSended = DateTime.Now;
                                if (k % 2 == 0)
                                {
                                    message.Sender = conversation3.FromUser;
                                    message.Text = "Hello friend for the" + k + "time";
                                }
                                else
                                {
                                    message.Sender = conversation3.ToUser;
                                    message.Text = "Hi there friend for the" + k + "time";
                                }
                                conversation3.Messages.Add(message);
                            }

                            context.Conversations.Add(conversation1);
                            context.Conversations.Add(conversation2);
                            context.Conversations.Add(conversation3);
                        }

                        var friend1 = new Friend();
                        friend1.ToUserAccepted = true;
                        friend1.FromUserAccepted = true;
                        var friend2 = new Friend();
                        friend2.ToUserAccepted = true;
                        friend2.FromUserAccepted = true;

                        users[i].FriendTo = new List<Friend>();
                        if (i > 5)
                        {
                            friend1.FromUser = users[i];
                            friend1.ToUser = users[1];
                            friend2.FromUser = users[i];
                            friend2.ToUser = users[2];
                            users[i].FriendTo.Add(friend1);
                            users[i].FriendTo.Add(friend2);
                        }
                    }
                }
                context.SaveChanges();
            }
            catch (Exception e)
            {
                throw e;
            }
        }
    }
}