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

export interface CourseAssignmentModel {
  id: number;
  title: string;
  createdAt: string;
}

export interface CourseModel {
  id: number;
  publicId: string;
  name: string;
  description?: string;
  category?: string;
  isTeacher: boolean;
  assignments: CourseAssignmentModel[];
}

export interface CourseUsersModel {
  teachers: CourseUserModel[];
  students: CourseUserModel[];
}

export interface CourseUserModel {
  courseUserId: number;
  fullname: string;
}

const getUserCourses = async (): Promise<GetUserCoursesResponseModel> => {
  const response = await axios.get<GetUserCoursesResponseModel>('/api/course/user-courses');
  return response.data;
};

const createCourse = async (course: CreateCourseRequestModel): Promise<CreateCourseResponseModel> => {
  const response = await axios.post<CreateCourseResponseModel>('/api/course', course);
  return response.data;
};

const getCourseDetails = async (courseId: number): Promise<CourseModel> => {
  const response = await axios.get<CourseModel>(`/api/course/${courseId}`);
  return response.data;
};

const joinCourse = async (publicId: string): Promise<void> => {
  await axios.post(`/api/course/${publicId}/join`);
};

const getCourseUsers = async (courseId: number): Promise<CourseUsersModel> => {
  const response = await axios.get<CourseUsersModel>(`/api/course/${courseId}/users`);
  return response.data;
};

const deleteStudentFromCourse = async (id: number): Promise<void> => {
  await axios.delete(`/api/course/student-course/${id}`);
};

export { getUserCourses, createCourse, getCourseDetails, joinCourse, getCourseUsers, deleteStudentFromCourse };
