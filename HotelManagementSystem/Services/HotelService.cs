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
        public async Task<RoomTypeDTO> AddRoomType([FromBody] RoomTypeDTO roomTypeDTO)
        {
            try
            {
                var roomType = _mapper.Map<RoomTypeDTO, RoomType>(roomTypeDTO);
                if (!await _unitOfWork.RoomType.AddAsync(roomType))
                {
                    throw new ApplicationException("An error occurred.");

                }
                return roomTypeDTO;

            }
            catch (Exception ex)
            {
                throw new ApplicationException("An error occurred." + ex);
            }
        }
        public async Task<HotelDTO> CreateHotel(HotelDTO hotelDTO)
        {
            try
            {
                hotelDTO.Images = "";
                //Images
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


                var addHotel = _mapper.Map<HotelDTO, Hotel>(hotelDTO);
                try
                {
                    await _unitOfWork.Hotel.AddAsync(addHotel);
                }
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
                                file.CopyTo(FileStream);  //save
                            }
                            if (item.Images == "")
                            {
                                item.Images = @"\images\hotels\" + fileName + extention;
                            }
                            else
                            {
                                item.Images = item.Images + "," + @"\images\hotels\" + fileName + extention;

                            }
                        }
                    }
                }
                var addHotelRooms = _mapper.Map<ICollection<HotelRoomDTO>, ICollection<HotelRoom>>(hotelRoomDTO);
                await _unitOfWork.HotelRoom.AddRangeAsync(addHotelRooms);
                return hotelRoomDTO;
            }
            catch (Exception ex)
            {
                throw new ApplicationException("An error occurred." + ex);
            }


        }

        public async Task<HotelRoomDTO> UpdateRoomInHotel(HotelRoomDTO hotelRoomDTO)
        {
            try
            {
                hotelRoomDTO.Images = "";
                if (hotelRoomDTO.Files != null)
                {
                    var webRootPath = _webHostEnvironment.WebRootPath;
                    var room = await _unitOfWork.HotelRoom.FirstOrDefaultAsync(r => r.Id == hotelRoomDTO.Id);
                    hotelRoomDTO.Images = room.Images;
                    //if (item.Images != null)
                    //{

                    //    string[] imagePathsArray = item.Images.Split(',');

                    //    foreach (var imagePath in imagePathsArray)
                    //    {
                    //        var trimmedPath = imagePath.Trim('\\');
                    //        var fullPath = Path.Combine(webRootPath, trimmedPath);

                    //        if (System.IO.File.Exists(fullPath))
                    //        {
                    //            System.IO.File.Delete(fullPath);
                    //        }
                    //    }
                    //    item.Images = "";
                    //}
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
                            hotelRoomDTO.Images = @"\images\hotels\" + fileName + extention;
                        }
                        else
                        {
                            hotelRoomDTO.Images = hotelRoomDTO.Images + "," + @"\images\hotels\" + fileName + extention;

                        }
                    }
                }
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

            public async Task<ICollection<HotelRoomDTO>> UpdateRoomsInHotel(ICollection<HotelRoomDTO> hotelRoomDTO)
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
                            var room = await _unitOfWork.HotelRoom.FirstOrDefaultAsync(r => r.Id == item.Id);
                            item.Images = room.Images;
                            //if (item.Images != null)
                            //{

                            //    string[] imagePathsArray = item.Images.Split(',');

                            //    foreach (var imagePath in imagePathsArray)
                            //    {
                            //        var trimmedPath = imagePath.Trim('\\');
                            //        var fullPath = Path.Combine(webRootPath, trimmedPath);

                            //        if (System.IO.File.Exists(fullPath))
                            //        {
                            //            System.IO.File.Delete(fullPath);
                            //        }
                            //    }
                            //    item.Images = "";
                            //}
                            foreach (var file in item.Files)
                            {
                                var fileName = Guid.NewGuid().ToString();
                                var extention = Path.GetExtension(file.FileName);
                                var upload = Path.Combine(webRootPath, @"images\hotels\rooms");

                                using (var FileStream = new FileStream(Path.Combine(upload, fileName + extention), FileMode.Create))
                                {
                                    file.CopyTo(FileStream);  //save
                                }
                                if (item.Images == "")
                                {
                                    item.Images = @"\images\hotels\" + fileName + extention;
                                }
                                else
                                {
                                    item.Images = item.Images + "," + @"\images\hotels\" + fileName + extention;

                                }
                            }
                        }
                        else
                        {
                            var room = await _unitOfWork.HotelRoom.FirstOrDefaultAsync(r => r.Id == item.Id);
                            item.Images = room.Images;
                        }
                        var hotelRooms = _mapper.Map<ICollection<HotelRoomDTO>, ICollection<HotelRoom>>(hotelRoomDTO);
                        await _unitOfWork.HotelRoom.UpdateRangeAsync(hotelRooms);
                    }
                    return hotelRoomDTO;
                }
                catch (Exception ex)
                {
                    throw new ApplicationException("An error occurred." + ex);
                }
            }

            public async Task<IEnumerable<HotelDTO>> GetHotels(int cityId)
            {
                try
                {
                    var hotels = await _unitOfWork.Hotel.GetAllAsync(h => h.CityId == cityId);
                    if (hotels == null)
                    {

                        return Enumerable.Empty<HotelDTO>();
                    }

                    var hotelsDTO = _mapper.Map<IEnumerable<HotelDTO>>(hotels);
                    return hotelsDTO;
                }
                catch (Exception)
                {

                    return Enumerable.Empty<HotelDTO>();
                }
            }
            public async Task<HotelVM> GetHotel(int id)
            {
                try
                {
                    var hotel = await _unitOfWork.Hotel.GetAsync(id);
                    if (hotel == null) return null;
                    var hotelVM = new HotelVM();
                    hotelVM.Hotel = _mapper.Map<HotelDTO>(hotel);
                    var hotelRooms = await _unitOfWork.HotelRoom.GetAllAsync(h => h.HotelId == hotelVM.Hotel.Id);
                    hotelVM.HotelRooms = _mapper.Map<List<HotelRoomDTO>>(hotelRooms);
                    return hotelVM;

                }
                catch (Exception ex)
                {
                    throw new ApplicationException("An error occurred." + ex);

                }
            }



        }
    }
