import axios from 'axios';

export interface AddAssignmentRequestModel {
  courseId: number;
  maxGrade?: number;
  title: string;
  description?: string;
  userId: number;
  deadline: string;
}

export const addAssignment = async (assignment: AddAssignmentRequestModel): Promise<void> => {
  await axios.post('/api/assignment', assignment);
};
