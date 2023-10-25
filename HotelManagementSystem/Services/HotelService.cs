using HotelManagementSystem.Models.DTO;
using HotelManagementSystem.Models.ViewModels;
using HotelManagementSystem.Models;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using AutoMapper;
using HotelManagementSystem.Repository.IRepository;
using HotelManagementSystem.ServiceContract;
using HotelManagementSystem.Data;
using System.Collections;
using HotelManagementSystem.Repository;

namespace HotelManagementSystem.Services
{
    public class HotelService : IHotelService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly IWebHostEnvironment _webHostEnvironment;
        private readonly ApplicationDbContext _context;

        public HotelService(IUnitOfWork unitOfWork, IMapper mapper, IWebHostEnvironment webHostEnvironment, ApplicationDbContext context)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _webHostEnvironment = webHostEnvironment;
            _context = context;
        }

        /// <summary>
        /// Adding a new Room Type in RoomType Entity
        /// </summary>
        /// <param name="roomTypeDTO"></param>
        /// <returns>Return the added object</returns>
        /// <exception cref="ApplicationException"></exception>
        public async Task<RoomTypeDTO> AddRoomType([FromBody] RoomTypeDTO roomTypeDTO)
        {
            try
            {
                //Map roomTypeDTO object to RoomType Entity
                var roomType = _mapper.Map<RoomTypeDTO, RoomType>(roomTypeDTO);

                //Insert room in database
                if (!await _unitOfWork.RoomType.AddAsync(roomType)) 
                {
                    throw new ApplicationException("An error occurred.");

                }
                //return the roomTypeDTO after success
                return roomTypeDTO; 

            }
            catch (Exception ex)
            {
                throw new ApplicationException("An error occurred." + ex);
            }
        }

        /// <summary>
        /// Adding a new Hotel in Hotel Entity, storing images in webroot and storing path of images in Database as string.
        /// </summary>
        /// <param name="hotelDTO"></param>
        /// <returns>Returns the added Hotel</returns>
        /// <exception cref="ApplicationException"></exception>
        public async Task<HotelDTO> CreateHotel(HotelDTO hotelDTO)
        {
            try
            {
                hotelDTO.Images = "";

                //Handling uploaded images
                if (hotelDTO.Files.Count > 0)
                {
                    var webRootPath = _webHostEnvironment.WebRootPath;
                    foreach (var file in hotelDTO.Files)
                    {
                        var fileName = Guid.NewGuid().ToString();
                        var extention = Path.GetExtension(file.FileName);
                        var upload = Path.Combine(webRootPath, @"images\hotels");

                        using (var FileStream = new FileStream(Path.Combine(upload, fileName + extention), FileMode.Create))
                        {
                            file.CopyTo(FileStream);  //save
                        }
                        if (hotelDTO.Images == "")
                        {
                            hotelDTO.Images = @"\images\hotels\" + fileName + extention;
                        }
                        else
                        {
                            hotelDTO.Images = hotelDTO.Images + "," + @"\images\hotels\" + fileName + extention;

                        }
                    }
                }

                //Map hotelDTO with the Entity
                var addHotel = _mapper.Map<HotelDTO, Hotel>(hotelDTO); 
                try
                {
                    await _unitOfWork.Hotel.AddAsync(addHotel); //adding mapped object in database
                }

                //catching exception in ex variable
                catch (Exception ex) 
                {
                    throw new ApplicationException("An error occurred." + ex);
                }
                return hotelDTO;
            }
            catch (Exception ex)
            {
                throw new ApplicationException("An error occurred." + ex);
            }
        }

        /// <summary>
        /// Adding details of the rooms available in the hotel
        /// </summary>
        /// <param name="hotelRoomDTO"></param>
        /// <returns>Collection of Rooms added in Hotel</returns>
        /// <exception cref="ApplicationException"></exception>
        public async Task<ICollection<HotelRoomDTO>> AddRoomsInHotel(ICollection<HotelRoomDTO> hotelRoomDTO)
        {
            try
            {
                //Images
                foreach (var item in hotelRoomDTO)
                {
                    item.Images = "";
                    if (item.Files != null)
                    {
                        var webRootPath = _webHostEnvironment.WebRootPath;
                        foreach (var file in item.Files)
                        {
                            var fileName = Guid.NewGuid().ToString();  
                            var extention = Path.GetExtension(file.FileName);
                            var upload = Path.Combine(webRootPath, @"images\hotels\rooms");

                            using (var FileStream = new FileStream(Path.Combine(upload, fileName + extention), FileMode.Create))
                            {
                                file.CopyTo(FileStream);  //save in folder
                            }
                            if (item.Images == "")
                            {
                                item.Images = @"\images\hotels\rooms\" + fileName + extention;
                            }
                            else
                            {
                                item.Images = item.Images + "," + @"\images\hotels\rooms\" + fileName + extention;

                            }
                        }
                    }
                }
                var addHotelRooms = _mapper.Map<ICollection<HotelRoomDTO>, ICollection<HotelRoom>>(hotelRoomDTO); //Mapping collection of hotelroomsDTo with Entity
                await _unitOfWork.HotelRoom.AddRangeAsync(addHotelRooms);  //adding range of hotelRooms in database
                return hotelRoomDTO;
            }
            catch (Exception ex)
            {
                throw new ApplicationException("An error occurred." + ex);
            }


        }

        /// <summary>
        /// Updating a specific roomtype in a specific Hotel
        /// </summary>
        /// <param name="hotelRoomDTO"></param>
        /// <returns>Returning room which is updated</returns>
        /// <exception cref="ApplicationException"></exception>
        public async Task<HotelRoomDTO> UpdateRoomInHotel(HotelRoomDTO hotelRoomDTO)
        {
            try
            {
                hotelRoomDTO.Images = "";

                //Handling images
                if (hotelRoomDTO.Files != null)
                {
                    var webRootPath = _webHostEnvironment.WebRootPath;
                    var room = await _unitOfWork.HotelRoom.FirstOrDefaultAsync(r => r.Id == hotelRoomDTO.Id);
                    hotelRoomDTO.Images = room.Images;
                    foreach (var file in hotelRoomDTO.Files)
                    {
                        var fileName = Guid.NewGuid().ToString();
                        var extention = Path.GetExtension(file.FileName);
                        var upload = Path.Combine(webRootPath, @"images\hotels\rooms");

                        using (var FileStream = new FileStream(Path.Combine(upload, fileName + extention), FileMode.Create))
                        {
                            file.CopyTo(FileStream);  //save
                        }
                        if (hotelRoomDTO.Images == "")
                        {
                            hotelRoomDTO.Images = @"\images\hotels\rooms\" + fileName + extention;
                        }
                        else
                        {
                            hotelRoomDTO.Images = hotelRoomDTO.Images + "," + @"\images\hotels\rooms\" + fileName + extention;

                        }
                    }
                }

                //if no images are selected
                else
                {
                    var room = await _unitOfWork.HotelRoom.FirstOrDefaultAsync(r => r.Id == hotelRoomDTO.Id);
                    hotelRoomDTO.Images = room.Images;
                }
                var hotelRoom = _mapper.Map<HotelRoomDTO, HotelRoom>(hotelRoomDTO);
                await _unitOfWork.HotelRoom.UpdateAsync(hotelRoom);


                return hotelRoomDTO;
            }
            catch (Exception ex)
            {
                throw new ApplicationException("An error occurred." + ex);
            }
        }

        /// <summary>
        /// Get hotels by City Id
        /// </summary>
        /// <param name="cityId"></param>
        /// <returns>Fetched Hotels</returns>
        public async Task<IEnumerable<HotelDTO>> GetHotels(int cityId)
        {
            try
            {
                //Get All Hotels where cityID is equals to desired city
                var hotels = await _unitOfWork.Hotel.GetAllAsync(hotels => hotels.CityId == cityId, includeProperties: "HotelRooms,City");
                if (hotels == null)
                {
                    return Enumerable.Empty<HotelDTO>();
                }

                //Mapping hotels to HotelDTO
                var hotelsDTO = _mapper.Map<IEnumerable<HotelDTO>>(hotels);
                return hotelsDTO;
            }
            catch (Exception)
            {

                return Enumerable.Empty<HotelDTO>();
            }
        }

        /// <summary>
        /// Get Details of particular Hotel
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        /// <exception cref="ApplicationException"></exception>
        public async Task<HotelDTO> GetHotel(int id)
        {
            try
            {
                //Fetch Hotel by Hotel Id
                var hotel = await _unitOfWork.Hotel.FirstOrDefaultAsync(hotel=>hotel.Id == id, includeProperties:"HotelRooms,HotelRooms.RoomType,City");
                if (hotel == null) return null;

                //Map the fetched hotel to HotelDTO
                var hotelDTO = _mapper.Map<HotelDTO>(hotel);
                return hotelDTO;

            }
            catch (Exception ex)
            {
                throw new ApplicationException("An error occurred." + ex);

            }
        }

        public async Task<HotelRoomDTO> GetHotelRoom(int id)
        {
            try
            {
                //Fetch HotelRoom by HotelRoom Id
                var hotelRoom = await _unitOfWork.HotelRoom.FirstOrDefaultAsync(hotelRoom => hotelRoom.Id == id, includeProperties: "Hotel,RoomType");
                if (hotelRoom == null) return null;

                //Map the fetched hotelRoom to HotelRoomDTO
                var hotelRoomDTO = _mapper.Map<HotelRoomDTO>(hotelRoom);
                return hotelRoomDTO;

            }
            catch (Exception ex)
            {
                throw new ApplicationException("An error occurred." + ex);

            }
        }



    }
}
