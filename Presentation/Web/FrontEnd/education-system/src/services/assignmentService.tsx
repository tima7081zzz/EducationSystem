import axios from 'axios';

export interface AddAssignmentRequestModel {
  courseId: number;
  maxGrade?: number;
  title: string;
  description?: string;
  userId: number;
  deadline: string;
}

export interface AttachmentModel {
    id: number;
    name: string;
}
  
  export interface AssignmentModel {
    id: number;
    title: string;
    description?: string;
    createdAt: string;
    deadline: string;
    maxGrade?: number;
    attachments: AttachmentModel[];
}

const getAssignmentDetails = async (assignmentId: number): Promise<AssignmentModel> => {
  const response = await axios.get<AssignmentModel>(`/api/assignment/${assignmentId}`);
  return response.data;
};

const addAssignment = async (assignment: AddAssignmentRequestModel): Promise<void> => {
  await axios.post('/api/assignment', assignment);
};

export { getAssignmentDetails, addAssignment };
