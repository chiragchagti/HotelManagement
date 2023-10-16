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

namespace HotelManagementSystem.Services
{
    public class HotelService : IHotelService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly IWebHostEnvironment _webHostEnvironment;

        public HotelService(IUnitOfWork unitOfWork, IMapper mapper, IWebHostEnvironment webHostEnvironment)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _webHostEnvironment = webHostEnvironment;
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
        public async Task<HotelVM> CreateHotel(HotelVM hotel)
        {
            try
            {

                if (hotel.files.Count > 0)
                {
                    //Images
                    var webRootPath = _webHostEnvironment.WebRootPath;
                    foreach (var file in hotel.files)
                    {
                        var fileName = Guid.NewGuid().ToString();
                        var extention = Path.GetExtension(file.FileName);
                        var upload = Path.Combine(webRootPath, @"images\hotels");

                        using (var FileStream = new FileStream(Path.Combine(upload, fileName + extention), FileMode.Create))
                        {
                            file.CopyTo(FileStream);  //save
                        }
                        if (hotel.Hotel.Images != null)
                        {
                            hotel.Hotel.Images = hotel.Hotel.Images + "," + @"\images\hotels\" + fileName + extention;

                        }
                        else
                        {
                            hotel.Hotel.Images = @"\images\hotels\" + fileName + extention;
                        }


                    }
                }
                var hotelVM = new HotelVM();
                hotelVM.Hotel = hotel.Hotel;

                var addHotel = _mapper.Map<HotelDTO, Hotel>(hotel.Hotel);
                try
                {
                    await _unitOfWork.Hotel.AddAsync(addHotel);


                }
                catch (Exception ex)
                {
                    throw new ApplicationException("An error occurred." + ex);
                }


                if (!await CreateHotelRooms(hotel))
                {
                    return null;
                }
                return hotel;
            }
            catch (Exception ex)
            {
                throw new ApplicationException("An error occurred." + ex);
            }


        }
        public async Task<bool> CreateHotelRooms(HotelVM hotel)
        {
            var hotelInserted = await _unitOfWork.Hotel.FirstOrDefaultAsync(h => h.Name == hotel.Hotel.Name && h.Address == hotel.Hotel.Address);
            foreach (var item in hotel.HotelRooms)
            {
                item.HotelId = hotelInserted.Id;
            }
            var addHotelRooms = _mapper.Map<List<HotelRoomDTO>, List<HotelRoom>>(hotel.HotelRooms);
            try
            {
                await _unitOfWork.HotelRoom.AddRangeAsync(addHotelRooms);
                return true;
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
