import React from 'react';
import { Paper, Typography, List, ListItem, ListItemText, Divider } from '@mui/material';

interface Assignment {
  id: number;
  title: string;
  creationDate: string;
}

interface AssignmentListProps {
  courseId: string;
  assignments: Assignment[];
  isTeacher: boolean;
}

const AssignmentList: React.FC<AssignmentListProps> = ({ courseId, assignments, isTeacher }) => {
  return (
    <Paper elevation={3} sx={{ padding: '16px' }}>
      <Typography variant="h6" gutterBottom>
        Assignments
      </Typography>
      <List>
        {assignments.map((assignment) => (
          <div key={assignment.id}>
            <ListItem button component="a" href={isTeacher ? `/course/${courseId}/assignment/${assignment.id}/overview` : `/course/${courseId}/assignment/${assignment.id}`}>
              <ListItemText primary={assignment.title} secondary={`Created on: ${assignment.creationDate}`} />
            </ListItem>
            <Divider />
          </div>
        ))}
      </List>
    </Paper>
  );
};

export default AssignmentList;
