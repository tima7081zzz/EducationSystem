import axios from '../axiosConfig';

export interface UserCourseModel {
  id: number;
  name: string;
  description?: string;
  creatorUserFullName: string;
  createdAt: string;
  isCreator: boolean;
  isTeacher: boolean;
}

export interface GetUserCoursesResponseModel {
  userCourses: UserCourseModel[];
}

export interface CreateCourseRequestModel {
  name: string;
  description?: string;
  category?: string;
}

export interface CreateCourseResponseModel {
  id: number;
}

const getUserCourses = async (): Promise<GetUserCoursesResponseModel> => {
  const response = await axios.get<GetUserCoursesResponseModel>('/api/course/user-courses');
  return response.data;
};

const createCourse = async (course: CreateCourseRequestModel): Promise<CreateCourseResponseModel> => {
  const response = await axios.post<CreateCourseResponseModel>('/api/course', course);
  return response.data;
};

export { getUserCourses, createCourse };
