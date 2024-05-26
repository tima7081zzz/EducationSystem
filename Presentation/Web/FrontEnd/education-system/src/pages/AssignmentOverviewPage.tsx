import React, { useEffect, useState } from 'react';
import { useParams } from 'react-router-dom';
import { Container, Grid, Paper, Typography, List, ListItem, ListItemText, TextField, Button, CircularProgress } from '@mui/material';
import { getAssignmentOverview, GetAssignmentOverviewModel, StudentAssignmentOverviewModel } from '../services/assignmentService';

const AssignmentOverviewPage: React.FC = () => {
  const { assignmentId } = useParams<{ assignmentId: string }>();
  const [overview, setOverview] = useState<GetAssignmentOverviewModel | null>(null);
  const [isLoading, setIsLoading] = useState<boolean>(true);
  const [error, setError] = useState<string | null>(null);

  useEffect(() => {
    const fetchAssignmentOverview = async () => {
      try {
        if (assignmentId) {
          const data = await getAssignmentOverview(Number(assignmentId));
          setOverview(data);
        }
      } catch (err) {
        setError('Failed to fetch assignment overview.');
      } finally {
        setIsLoading(false);
      }
    };

    fetchAssignmentOverview();
  }, [assignmentId]);

  const handleGradeChange = (studentId: number, grade: string) => {
    if (overview) {
      const updatedStudents = overview.studentAssignmentInfos.map((student) =>
        student.userId === studentId ? { ...student, grade: Number(grade) } : student
      );
      setOverview({ ...overview, studentAssignmentInfos: updatedStudents });
    }
  };

  if (isLoading) {
    return <Typography>Loading...</Typography>;
  }

  if (error) {
    return <Typography color="error">{error}</Typography>;
  }

  if (!overview) {
    return <Typography>Assignment not found.</Typography>;
  }

  return (
    <Container>
      <Grid container spacing={3}>
        <Grid item xs={12} md={4}>
          <Paper elevation={3} sx={{ padding: '16px' }}>
            <Typography variant="h5" gutterBottom>
              {overview.title}
            </Typography>
            <Typography>Submitted: {overview.submittedCount}</Typography>
            <Typography>Not Submitted: {overview.notSubmittedCount}</Typography>
            <Typography>Graded: {overview.gradedCount}</Typography>
          </Paper>
        </Grid>
        <Grid item xs={12} md={8}>
          <Paper elevation={3} sx={{ padding: '16px' }}>
            <Typography variant="h5" gutterBottom>
              Student Assignments
            </Typography>
            <List>
              {overview.studentAssignmentInfos.map((student) => (
                <ListItem key={student.userId} sx={{ display: 'flex', alignItems: 'center' }}>
                  <ListItemText primary={student.userFullname} />
                  <TextField
                    type="number"
                    label={`Grade (0-${overview.maxGrade})`}
                    value={student.grade ?? ''}
                    onChange={(e) => handleGradeChange(student.userId, e.target.value)}
                    sx={{ maxWidth: 100, mr: 2 }}
                  />
                  <Typography variant="body2">
                    {student.grade ?? 'Not graded'} / {overview.maxGrade}
                  </Typography>
                </ListItem>
              ))}
            </List>
          </Paper>
        </Grid>
      </Grid>
    </Container>
  );
};

export default AssignmentOverviewPage;
