﻿using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using SportBooking.Server.Dto;
using SportBooking.Server.Interfaces;
using SportBooking.Server.models;
using SportBooking.Server.Repository;

namespace SportBooking.Server.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class CourtController : Controller
    {
        private readonly ICourtRepository _courtRepository;
        private readonly IMapper _mapper;
        public CourtController(ICourtRepository courtRepository, IMapper mapper)
        {
            _courtRepository = courtRepository;
            _mapper = mapper;
        }

        [HttpGet("GetListCourts")]
        [ProducesResponseType(200, Type = typeof(ICollection<Court>))]
        public async Task<IActionResult> GetCourts()
        {
            var courts = await _courtRepository.GetCourts();
            if (courts == null)
                return BadRequest("Not found courts");
            var newCourts = _mapper.Map<ICollection<CourtigCourtTimeSlotDto>>(courts);
            return Ok(newCourts);
        }

        [HttpGet("GetCourt/{id}")]
        [ProducesResponseType(200, Type = typeof(Court))]
        public async Task<IActionResult> GetCourt(int id)
        {
            var court = await _courtRepository.GetCourt(id);
            if (court == null)
                return NotFound("Not found court");
            var newCourt = _mapper.Map<CourtigCourtTimeSlotDto>(court);
            return Ok(newCourt);
        }

        [HttpPost("CreateCourt/{id}")]
        [ProducesResponseType(200, Type = typeof(bool))]
        public async Task <IActionResult> CreateCourt(int id,[FromBody]CourtDto court)
        {
            var newCourt = await _courtRepository.AddCourt(id,_mapper.Map<Court>(court));
            if (newCourt == null)
                return BadRequest("Add court Failed");
            var _newCourt = _mapper.Map<CourtIdDto>(newCourt);
            return Ok(_newCourt);
        }

        [HttpPut("UpdateCourt")]
        [ProducesResponseType(200, Type = typeof(Court))]
        public async Task<IActionResult> UpdateCourt([FromHeader] int id,[FromBody]CourtDto court)
        {
            var oldCourt = await _courtRepository.GetCourt(id);
            if (oldCourt == null)
                return BadRequest("Have not court");
            var newCourt = _mapper.Map(court, oldCourt);
            var courtRes = await _courtRepository.UpdateCourt(_mapper.Map<Court>(newCourt));
            if (courtRes == null)
                return BadRequest("Update court Failed");
            var _courtRes = _mapper.Map<CourtIdDto>(courtRes);
            return Ok(_courtRes);
        }

        [HttpDelete("DeleteCourt/{id}")]
        [ProducesResponseType(200, Type = typeof(bool))]
        public async Task<IActionResult> DeleteCourt(int id)
        {
            var result = await _courtRepository.DeleteCourt(id);
            if (!result)
                return BadRequest("Delete court Failed");
            return Ok(result);
        }
    }


}