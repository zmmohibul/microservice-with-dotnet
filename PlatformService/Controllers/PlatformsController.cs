using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using PlatformService.Data;
using PlatformService.DTOs;
using PlatformService.Models;

namespace DefaultNamespace
{
    [ApiController]
    [Route("api/[controller]")]
    public class PlatformsController : ControllerBase
    {
        private readonly IPlatformRepository _platformRepository;
        private readonly IMapper _mapper;

        public PlatformsController(IPlatformRepository platformRepository, IMapper mapper)
        {
            _platformRepository = platformRepository;
            _mapper = mapper;
        }

        [HttpGet]
        public async Task<IActionResult> GetPlatforms()
        {
            var platforms = await _platformRepository.GetAllPlatforms();

            return Ok(_mapper.Map<IEnumerable<PlatformReadDto>>(platforms));
        }

        [HttpGet("{id}", Name = "GetPlatformById")]
        public async Task<IActionResult> GetPlatformById(int id)
        {
            var platform = await _platformRepository.GetPlatformById(id);
            if (platform == null)
            {
                return NotFound();
            }
            else
            {
                return Ok(_mapper.Map<PlatformReadDto>(platform));
            }
        }

        [HttpPost]
        public async Task<IActionResult> CreatePlatform(PlatformCreateDto platformCreateDto)
        {
            var platform = _mapper.Map<Platform>(platformCreateDto);
            _platformRepository.CreatePlatform(platform);
            await _platformRepository.SaveChanges();

            var platformToReturn = _mapper.Map<PlatformReadDto>(platform);

            return CreatedAtRoute(nameof(GetPlatformById), new {Id = platformToReturn.Id}, platformToReturn);
        }
    }
}