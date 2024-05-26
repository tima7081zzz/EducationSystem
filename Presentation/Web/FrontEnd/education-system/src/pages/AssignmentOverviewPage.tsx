import React, { useEffect, useState } from 'react';
import { useParams } from 'react-router-dom';
import {
  Container,
  Grid,
  Paper,
  Typography,
  List,
  ListItem,
  ListItemText,
  TextField,
  Button,
  Dialog,
  DialogTitle,
  DialogContent,
  DialogActions,
  Divider,
  Box
} from '@mui/material';
import { getAssignmentOverview, GetAssignmentOverviewModel, StudentAssignmentOverviewModel } from '../services/assignmentService';
import axios from 'axios';

const AssignmentOverviewPage: React.FC = () => {
  const { assignmentId } = useParams<{ assignmentId: string }>();
  const [overview, setOverview] = useState<GetAssignmentOverviewModel | null>(null);
  const [isLoading, setIsLoading] = useState<boolean>(true);
  const [error, setError] = useState<string | null>(null);
  const [openDialog, setOpenDialog] = useState<boolean>(false);
  const [selectedStudent, setSelectedStudent] = useState<StudentAssignmentOverviewModel | null>(null);
  const [gradingComment, setGradingComment] = useState<string>('');

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

  const handleOpenDialog = (student: StudentAssignmentOverviewModel) => {
    setSelectedStudent(student);
    setGradingComment('');
    setOpenDialog(true);
  };

  const handleCloseDialog = () => {
    setOpenDialog(false);
    setSelectedStudent(null);
  };

  const handleSubmitGrade = async () => {
    if (selectedStudent && overview) {
      const grade = selectedStudent.grade ?? 0;
      const request = {
        grade,
        gradingComment
      };

      try {
        await axios.post(`/api/assignment/${overview.id}/student-user/${selectedStudent.userId}/grade`, request);
        alert('Grade submitted successfully.');
        handleCloseDialog();
      } catch (err) {
        alert('Failed to submit grade.');
      }
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
              Student Assignments
            </Typography>
            <List>
              {overview.studentAssignmentInfos.map((student) => (
                <ListItem key={student.userId} sx={{ display: 'flex', alignItems: 'center' }}>
                  <ListItemText primary={student.userFullname} sx={{ flex: 1 }} />
                  <TextField
                    type="number"
                    value={student.grade ?? ''}
                    onChange={(e) => handleGradeChange(student.userId, e.target.value)}
                    sx={{ maxWidth: 80, mr: 2 }}
                    size="small"
                  />
                  <Typography variant="body2" sx={{ mr: 2 }}>
                    {student.grade ?? 'Not graded'} / {overview.maxGrade}
                  </Typography>
                  <Button variant="contained" color="primary" onClick={() => handleOpenDialog(student)}>
                    Grade
                  </Button>
                </ListItem>
              ))}
            </List>
          </Paper>
        </Grid>
        <Grid item xs={12} md={8}>              
          <Paper elevation={3} sx={{ padding: '16px', display: 'flex', justifyContent: 'space-between', alignItems: 'center' }}>
          <Box>
            <Typography variant="h5" gutterBottom >
              {overview.title}
            </Typography>
            <Box sx={{ display: 'flex', alignItems: 'center', justifyContent: 'start', width: '100%' }}>
              <Box sx={{ textAlign: 'center' }}>
                <Typography variant="body2">Submitted</Typography>
                <Typography variant="h5">{overview.submittedCount}</Typography>
              </Box>
              <Divider orientation="vertical" flexItem sx={{ mx: 2, backgroundColor: 'blue' }} />
              <Box sx={{ textAlign: 'center' }}>
                <Typography variant="body2">Not Submitted</Typography>
                <Typography variant="h5">{overview.notSubmittedCount}</Typography>
              </Box>
              <Divider orientation="vertical" flexItem sx={{ mx: 2, backgroundColor: 'blue' }} />
              <Box sx={{ textAlign: 'center' }}>
                <Typography variant="body2">Graded</Typography>
                <Typography variant="h5">{overview.gradedCount}</Typography>
              </Box>
            </Box>
            </Box>
          </Paper>
        </Grid>
      </Grid>

      <Dialog open={openDialog} onClose={handleCloseDialog}>
        <DialogTitle>Submit Grade</DialogTitle>
        <DialogContent>
          <TextField
            label="Grading Comment"
            multiline
            rows={4}
            value={gradingComment}
            onChange={(e) => setGradingComment(e.target.value)}
            fullWidth
            variant="outlined"
            margin="dense"
          />
        </DialogContent>
        <DialogActions>
          <Button onClick={handleCloseDialog} color="primary">
            Cancel
          </Button>
          <Button onClick={handleSubmitGrade} color="primary">
            Submit
          </Button>
        </DialogActions>
      </Dialog>
    </Container>
  );
};

export default AssignmentOverviewPage;
