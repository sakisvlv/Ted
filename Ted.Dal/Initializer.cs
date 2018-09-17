using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ted.Model.Auth;
using Ted.Model.PersonalSkills;
using Ted.Model.Posts;

namespace Ted.Dal
{
    public static class Initializer
    {
        public async static Task Initialize(Context context, UserManager<User> userManager, RoleManager<Role> roleManager)
        {
            try
            {


                context.Database.EnsureCreated();

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
                    u.CurrentState = "Chief at X-men";
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
                        experiance.Title = "Chief";
                        experiance.Company = "X-men";
                        experiance.Description = "Killing monsters";
                        users[i].Experiences.Add(experiance);

                        var personalSkill = new PersonalSkill();
                        personalSkill.IsPublic = true;
                        personalSkill.Title = "Laser Eyes";
                        personalSkill.Description = "Shooting lasers out of the eyes";
                        users[i].PersonalSkills.Add(personalSkill);

                        for (int j = 0; j < 5; j++)
                        {
                            var post = new Post();
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
                                var comment2 = new Comment();
                                var comment3 = new Comment();
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
                            post.Type = Model.PostType.Title;
                            context.Posts.Add(post);
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