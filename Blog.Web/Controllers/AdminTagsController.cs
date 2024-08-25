﻿using Blog.Web.Data;
using Blog.Web.Models.Domain;
using Blog.Web.Models.ViewModels;
using Blog.Web.Repositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;

namespace Blog.Web.Controllers
{
    public class AdminTagsController : Controller
    {
        private readonly ITagRepository tagRepository;

        public AdminTagsController(ITagRepository tagRepository)
        {
            this.tagRepository = tagRepository;
        }


        [Authorize]
        [HttpGet]
        public IActionResult Add()
        {
            return View();
        }

        [HttpPost]
        [ActionName("Add")]
        public async Task< IActionResult> Add(AddTagRequest addTagRequest)
        {
            var tag = new Tag
            {
                Name = addTagRequest.Name,
                DisplayName = addTagRequest.DisplayName
            };

            await tagRepository.AddAsync(tag);

            TempData["Success"] = "New Tag is added!";
            return RedirectToAction("List");
        }

        [HttpGet]
        [ActionName("List")]
        public async Task<IActionResult> List()
        {
            var tags = await tagRepository.GetAllAsync();

            return View(tags);
        }

        public async Task<IActionResult> Edit(Guid id)
        {
         var tag = await tagRepository.GetAsync(id);

            if (tag != null) 
            {
                var editTagRequest = new EditTagRequest
                {
                    Id = tag.Id,
                    Name = tag.Name,
                    DisplayName = tag.DisplayName
                };
                return View(editTagRequest);
            }

            return View(null);
        }
        [HttpPost]
        public async Task<IActionResult> Edit (EditTagRequest editTagRequest)
        {
            var tag = new Tag
            {
                Id = editTagRequest.Id,
                Name = editTagRequest.Name,
                DisplayName = editTagRequest.DisplayName
            };

           var updatedTag = await tagRepository.UpdateAsync(tag);

            if (updatedTag != null)
            {
                //show success notification
                TempData["Success"] = "Tag is Edited!";
            }
            else
            {
                //show error notification
                TempData["Error"] = "Somwthing went wrong!";
            }
        
            return RedirectToAction("Edit", new { id = editTagRequest.Id });
        }

        [HttpPost]
        public async Task<IActionResult> Delete(EditTagRequest editTagRequest)
        {
         var deletedTag = await tagRepository.DeleteAsync(editTagRequest.Id);
            

            if (deletedTag != null)
            {
                //show success notification
                return RedirectToAction("List");
            }
            return RedirectToAction("Edit", new { id = editTagRequest });
        }

    }
}
