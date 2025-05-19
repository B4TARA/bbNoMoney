using KOP.BLL.Interfaces;
using KOP.Common.DTOs;
using KOP.Common.DTOs.AssessmentDTOs;
using KOP.Common.Enums;
using KOP.Common.Interfaces;
using KOP.DAL.Interfaces;

namespace KOP.BLL.Services
{
    public class AnalyticsService : IAnalyticsService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IAssessmentService _assessmentService;
        private readonly ISupervisorService _supervisorService;

        public AnalyticsService(IUnitOfWork unitOfWork, IAssessmentService assessmentService, ISupervisorService supervisorService)
        {
            _unitOfWork = unitOfWork;
            _assessmentService = assessmentService;
            _supervisorService = supervisorService;
        }

        public async Task<IBaseResponse<List<AssessmentDTO>>> GetEmployeeAssessmentAnalytics(int employeeId)
        {
            try
            {
                var employee = await _unitOfWork.Employees.GetAsync(x => x.Id == employeeId, includeProperties: new string[]
                {
                    "Assessments"
                });

                var assessmentDTOs = new List<AssessmentDTO>();

                foreach (var assessment in employee.Assessments)
                {
                    var assessmentDTO = new AssessmentDTO
                    {
                        Id = assessment.Id,
                        Number = assessment.Number,
                        EmployeeId = assessment.EmployeeId,
                        SystemStatus = assessment.SystemStatus,
                        NextAssessmentDate = assessment.NextAssessmentDate,
                    };

                    // Для каждого результата качественной оценки
                    foreach (var assessmentResult in assessment.AssessmentResults)
                    {
                        var assessmentResultDTO = new AssessmentResultDTO
                        {
                            Id = assessmentResult.Id,
                            SystemStatus = assessmentResult.SystemStatus,
                            Sum = assessmentResult.AssessmentResultValues.Sum(x => x.Value),
                        };

                        assessmentResultDTO.Judge = new EmployeeDTO
                        {
                            Id = assessmentResult.Judge.Id,
                            ImagePath = assessmentResult.Judge.ImagePath,
                            FullName = assessmentResult.Judge.FullName,
                        };

                        assessmentResultDTO.Judged = new EmployeeDTO
                        {
                            Id = assessment.EmployeeId,
                        };

                        foreach (var value in assessmentResult.AssessmentResultValues)
                        {
                            assessmentResultDTO.Values.Add(new AssessmentResultValueDTO
                            {
                                Value = value.Value,
                                AssessmentMatrixRow = value.AssessmentMatrixRow,
                            });
                        }

                        foreach (var element in assessment.AssessmentType.AssessmentMatrix.Elements)
                        {
                            assessmentResultDTO.Elements.Add(new AssessmentMatrixElementDTO
                            {
                                Row = element.Row,
                                Value = element.Value,
                            });
                        }

                        assessmentDTO.AssessmentResults.Add(assessmentResultDTO);
                    }

                    assessmentDTOs.Add(assessmentDTO);
                }

                return new BaseResponse<List<AssessmentDTO>>()
                {
                    Data = assessmentDTOs,
                    StatusCode = StatusCodes.OK
                };
            }
            catch (Exception ex)
            {
                return new BaseResponse<List<AssessmentDTO>>()
                {
                    Description = $"[AnalyticsService.GetEmployeeAssessmentAnalytics] : {ex.Message}",
                    StatusCode = StatusCodes.InternalServerError,
                };
            }
        }
    }
}