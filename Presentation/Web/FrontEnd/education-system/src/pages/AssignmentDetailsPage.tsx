import React, { useEffect, useState } from 'react';
import { Container, Typography, Grid, Paper, Box, Button, TextField, CircularProgress, List, ListItem, ListItemText, ListItemIcon } from '@mui/material';
import { useParams } from 'react-router-dom';
import { getAssignmentDetails, uploadAttachment, AssignmentModel } from '../services/assignmentService';
import AttachFileIcon from '@mui/icons-material/AttachFile';

const AssignmentDetailsPage: React.FC = () => {
  const { assignmentId } = useParams<{ assignmentId: string }>();
  const [assignment, setAssignment] = useState<AssignmentModel | null>(null);
  const [isLoading, setIsLoading] = useState<boolean>(true);
  const [error, setError] = useState<string | null>(null);
  const [submissionText, setSubmissionText] = useState<string>('');
  const [attachment, setAttachment] = useState<File | null>(null);

  useEffect(() => {
    const fetchAssignmentDetails = async () => {
      try {
        if (assignmentId) {
          const data = await getAssignmentDetails(Number(assignmentId));
          setAssignment(data);
        }
      } catch (err) {
        setError('Failed to fetch assignment details.');
      } finally {
        setIsLoading(false);
      }
    };

    fetchAssignmentDetails();
  }, [assignmentId]);

  const handleTextChange = (e: React.ChangeEvent<HTMLInputElement>) => {
    setSubmissionText(e.target.value);
  };

  const handleFileChange = async (e: React.ChangeEvent<HTMLInputElement>) => {
    const files = e.target.files;
    if (!files || files.length === 0) return;

    const file = files[0];
    if (file.size > 10 * 1024 * 1024) {
      setError('File size should not exceed 10MB');
      setAttachment(null);
    } else {
      setError(null);
      setAttachment(file || null);

      // Upload attachment immediately after selection
      try {
        await uploadAttachment(Number(assignmentId), file);
        // Update assignment details to include the newly uploaded attachment
        const updatedAssignment = await getAssignmentDetails(Number(assignmentId));
        setAssignment(updatedAssignment);
        setAttachment(null); // Clear the selected file after upload
      } catch (error) {
        console.error('Failed to upload attachment:', error);
        alert('Failed to upload attachment. Please try again later.');
      }
    }
  };

  const handleSubmit = async () => {
    if (submissionText.length > 300) {
      setError('Submission text should not exceed 300 characters');
      return;
    }
    setError(null);
    // Handle submission logic here
    console.log('Submitting assignment with text:', submissionText, 'and attachment:', attachment);
  };

  if (isLoading) {
    return <CircularProgress />;
  }

  if (error) {
    return <Typography color="error">{error}</Typography>;
  }

  if (!assignment) {
    return <Typography>Assignment not found.</Typography>;
  }

  return (
    <Container>
      <Grid container spacing={3}>
        {/* Left Section: Assignment Details */}
        <Grid item xs={12} md={8}>
          <Paper elevation={3} sx={{ padding: '16px', mb: 3 }}>
            <Box>
              <Typography variant="h5" component="h1" gutterBottom>
                {assignment.title}
              </Typography>
              <Typography variant="body1" component="p" gutterBottom>
                {assignment.description}
              </Typography>
              <Typography variant="body2" color="textSecondary" gutterBottom>
                Created on: {new Date(assignment.createdAt).toLocaleDateString()}
              </Typography>
              <Typography variant="body2" color="textSecondary" gutterBottom>
                Deadline: {new Date(assignment.deadline).toLocaleDateString()}
              </Typography>
              {assignment.maxGrade && (
                <Typography variant="body2" color="textSecondary" gutterBottom>
                  Max Grade: {assignment.maxGrade}
                </Typography>
              )}
            </Box>
          </Paper>
        </Grid>

        {/* Right Section: Attachments and Submissions */}
        <Grid item xs={12} md={4}>
          <Paper elevation={3} sx={{ padding: '16px', mb: 3 }}>
              <Typography variant="h6" gutterBottom>
                Attachments
              </Typography>
              <List>
                {assignment.attachments.map((attachment) => (
                  <ListItem key={attachment.id}>
                    <ListItemIcon>
                      <AttachFileIcon />
                    </ListItemIcon>
                    <ListItemText primary={attachment.name} />
                  </ListItem>
                ))}
              </List>
            <Box component="form" noValidate autoComplete="off" sx={{ display: 'flex', flexDirection: 'column', gap: 2 }}>
              <Button
                variant="contained"
                component="label"
              >
                Upload Attachment
                <input
                  type="file"
                  hidden
                  onChange={handleFileChange}
                />
              </Button>
              {attachment && <Typography variant="body2">{attachment.name}</Typography>}
              <TextField
                label="Submission Text"
                multiline
                rows={4}
                value={submissionText}
                onChange={handleTextChange}
                inputProps={{ maxLength: 300 }}
              />
              {error && <Typography color="error">{error}</Typography>}
              <Button
                variant="contained"
                color="primary"
                onClick={handleSubmit}
              >
                Submit
              </Button>
            </Box>
          </Paper>
        </Grid>
      </Grid>
    </Container>
  );
};

export default AssignmentDetailsPage;
